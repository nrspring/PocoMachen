using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.Services
{
    public class TemplateServices
    {
        private List<PocoMachen.Integration.ITemplateEngine> GetTemplatesFromAssembly(string path)
        {
            var returnList = new List<PocoMachen.Integration.ITemplateEngine>();

            var assembly = Assembly.LoadFrom(path);
            var type = typeof(PocoMachen.Integration.ITemplateEngine);

            var classTypes = assembly.GetTypes().Where(p => type.IsAssignableFrom(p));

            foreach (var current in classTypes)
            {
                returnList.Add((PocoMachen.Integration.ITemplateEngine)Activator.CreateInstance(current));
            }

            return returnList;
        }

        public PocoMachen.Integration.ITemplateEngine GetTemplate(string path, string templateName)
        {
            var templates = GetTemplatesFromAssembly(path);
            var template =templates.FirstOrDefault(x => x.GetTemplateName().Equals(templateName, StringComparison.InvariantCultureIgnoreCase));

            if(template==null) throw new Exception("The given template does not exist");

            return template;
        }

        public void ExecuteTemplate(Dictionary<string, string> commandlineArguments,List<PocoMachen.Integration.Models.Table> tables)
        {
            var template = GetTemplate(commandlineArguments["templateassembly"], commandlineArguments["templatename"]);
            template.Execute(commandlineArguments, tables);
        }
    }
}
