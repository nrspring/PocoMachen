using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PocoMachen.Integration.Models;

namespace SimpleSql.Template.SimpleSql
{
    public class SimpleSql : PocoMachen.Integration.ITemplateEngine
    {
        public string GetTemplateName()
        {
            return "simplesql";
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
                    BuildCodeOutput(commandLineArguments, tables);
                    break;
                case "assembly":
                    //BuildAssemblyOutput(commandLineArguments, tables);
                    break;
                default:
                    throw new Exception("An invalid output type was selected for this template.  Valid types are 'code' or 'assembly'");
            }
        }

        private void BuildCodeOutput(Dictionary<string, string> commandLineArguments, List<Table> tables)
        {
            CheckForNecessaryConfiguration(commandLineArguments, "outputpath", "This argument is used to define to where the output will be written");
            CheckForNecessaryConfiguration(commandLineArguments, "namespace", "This argument is used to define the namespace for the created classes");

            CreateOutputDirectory(commandLineArguments);

            foreach (var current in tables)
            {
                string tableCode = BuildCodeForTable(current);
                string wrappedTableCode = WrapInNamespace(commandLineArguments["namespace"], tableCode);
                string filename = $"{current.Name}.cs";
                string fullPath = System.IO.Path.Combine(commandLineArguments["outputpath"], filename);

                System.IO.File.WriteAllText(fullPath, wrappedTableCode);
            }
        }

        private string BuildCodeForTable(PocoMachen.Integration.Models.Table table)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"public class {table.Name} : BaseTable {{");

            sb.AppendLine($"public {table.Name}() {{");
            sb.AppendLine("Select = \"*\";");
            sb.AppendLine($"From = \"{table.Name}\";");

            foreach (var current in table.Columns)
            {
                sb.AppendLine($"\t{current.Name} = new {GetFieldType(current)}(this,\"{current.Name}\");");
            }

            sb.AppendLine("}");

            foreach (var current in table.Columns)
            {
                sb.AppendLine($"\tpublic {GetFieldType(current)} {current.Name} {{ get; set; }}");
            }

            sb.AppendLine("}");

            return sb.ToString();
        }

        private string GetFieldType(PocoMachen.Integration.Models.Column column)
        {
            switch (column.DotNetType)
            {
                case "System.DateTime":
                    return "DateField";
                case "System.DateTime?":
                    return "DateFieldNullable";
                case "System.Int32":
                    return "IntField";
                case "System.Int32?":
                    return "IntFieldNullable";
                case "System.Double":
                    return "DoubleField";
                case "System.Double?":
                    return "DoubleFieldNullable";
            }

            return "StringField";
        }

        private void WriteSupportFile(Dictionary<string, string> commandLineArguments, string fileName, string body)
        {
            string fullPath = System.IO.Path.Combine(commandLineArguments["outputpath"], fileName);
            System.IO.File.WriteAllText(fullPath, WrapInNamespace(commandLineArguments["namespace"], body));
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
            if (!System.IO.Directory.Exists(commandLineArguments["outputpath"]))
                System.IO.Directory.CreateDirectory(commandLineArguments["outputpath"]);
        }
    }
}
