using System.Collections.Generic;

namespace PocoMachen.Integration
{
    public interface ITemplateEngine
    {
        string GetTemplateName();
        void Execute(Dictionary<string, string> commandLineArguments, List<Integration.Models.Table> tables);
    }
}