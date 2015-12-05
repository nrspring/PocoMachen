using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var arguments = (new Services.EnvironmentServices()).GetCommandLineArguments();
                if (!arguments.ContainsKey("provider")) throw new Exception("A provider must be defined");

                var provider = (new Services.ProviderServices()).GetProvider(arguments["provider"]);
                if (provider == null) throw new Exception("The given provider does not exist");

                var tables = provider.ProviderClass.GetTables(arguments);

                (new Services.TemplateServices()).ExecuteTemplate(arguments,tables);

                return 0;
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex.Message);
                return -1;
            }

        }

        private static void WriteErrorMessage(string errorMessage)
        {
            Console.WriteLine($"*** Error *** {errorMessage}");
        }
    }
}
