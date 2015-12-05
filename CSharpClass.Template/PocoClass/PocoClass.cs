using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using PocoMachen.Integration.Models;

namespace CSharpClass.Template.PocoClass
{
    public class PocoClass : PocoMachen.Integration.ITemplateEngine
    {
        public string GetTemplateName()
        {
            return "PocoClass";
        }

        public void Execute(Dictionary<string, string> commandLineArguments, List<Table> tables)
        {
            string effectiveOutputType = "code";
            if (commandLineArguments.ContainsKey("outputtype"))
            {
                effectiveOutputType = commandLineArguments["outputtype"];
            }

            switch (effectiveOutputType)
            {
                case "code":
                    BuildCodeOutput(commandLineArguments,tables);
                    break;
                case "assembly":
                    BuildAssemblyOutput(commandLineArguments, tables);
                    break;
                default:
                    throw new Exception("An invalid output type was selected for this template.  Valid types are 'code' or 'assembly'");
            }
        }

        private void BuildCodeOutput(Dictionary<string, string> commandLineArguments, List<Table> tables)
        {
            CheckForNecessaryConfiguration(commandLineArguments,"outputpath","This argument is used to define to where the output will be written");
            CheckForNecessaryConfiguration(commandLineArguments, "namespace", "This argument is used to define the namespace for the created classes");

            CreateOutputDirectory(commandLineArguments);

            foreach (var current in tables)
            {
                string tableCode = BuildCodeForTable(current);
                string wrappedTableCode = WrapInNamespace(commandLineArguments["namespace"], tableCode);
                string filename = $"{current.Name}.cs";
                string fullPath = System.IO.Path.Combine(commandLineArguments["outputpath"], filename);

                System.IO.File.WriteAllText(fullPath,wrappedTableCode);
            }
        }

        private void BuildAssemblyOutput(Dictionary<string, string> commandLineArguments, List<Table> tables)
        {
            CheckForNecessaryConfiguration(commandLineArguments, "outputpath",
                "This argument is used to define to where the output will be written");
            CheckForNecessaryConfiguration(commandLineArguments, "namespace",
                "This argument is used to define the namespace for the created classes");
            CheckForNecessaryConfiguration(commandLineArguments, "outputassemblyname",
                "This argument is used to define the file name of the output dll");

            CreateOutputDirectory(commandLineArguments);

            var sb = new StringBuilder();

            foreach (var current in tables)
            {
                sb.AppendLine(BuildCodeForTable(current));
            }
        

            string wrappedTableCode = WrapInNamespace(commandLineArguments["namespace"], sb.ToString());
            string fullPath = System.IO.Path.Combine(commandLineArguments["outputpath"], commandLineArguments["outputassemblyname"]);

            var provider = new CSharpCodeProvider();
            var options = new CompilerParameters
            {
                OutputAssembly = fullPath
            };

            var compilerOutput = provider.CompileAssemblyFromSource(options, new[] { wrappedTableCode });
            if (compilerOutput.Errors.HasErrors)
            {
                var sbError = new StringBuilder();

                foreach (var current in compilerOutput.Errors)
                {
                    sbError.AppendLine(current.ToString());
                }

                throw new Exception(sbError.ToString());
            }
        }

        private string BuildCodeForTable(PocoMachen.Integration.Models.Table table)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"public class {table.Name} {{");

            foreach (var current in table.Columns)
            {
                sb.AppendLine($"\tpublic {current.DotNetType} {current.Name} {{ get; set; }}");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private string WrapInNamespace(string outputNamespace, string body)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"namespace {outputNamespace}");
            sb.AppendLine("{");
            sb.AppendLine(body);
            sb.AppendLine("}");

            return sb.ToString();
        }

        private void CheckForNecessaryConfiguration(Dictionary<string, string> commandLineArguments, string key, string message)
        {
            if (!commandLineArguments.ContainsKey(key)) throw new Exception($"The '{key}' argument must be provided for this template. {message}");
        }

        private void CreateOutputDirectory(Dictionary<string, string> commandLineArguments)
        {
            if(!System.IO.Directory.Exists(commandLineArguments["outputpath"]))
                System.IO.Directory.CreateDirectory(commandLineArguments["outputpath"]);
        }
    }
}
