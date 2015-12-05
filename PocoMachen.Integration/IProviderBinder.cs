using System.Collections.Generic;

namespace PocoMachen.Integration
{
    public interface IProviderBinder
    {
        string GetProviderName();
        List<Models.Table> GetTables(Dictionary<string,string> commandLineArgs);
    }
}