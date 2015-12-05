using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.Services
{
    public class ProviderServices
    {
        private const string ProviderFileExtenstion = "PocoMachenProvider.dll";

        private List<string> GetProvidersFiles()
        {
            var returnList = new List<string>();

            string applicationPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string applicationDirectory = System.IO.Path.GetDirectoryName(applicationPath);
            var directory = new System.IO.DirectoryInfo(applicationDirectory);

            foreach (var current in directory.GetFiles($"*.{ProviderFileExtenstion}"))
            {
                returnList.Add(current.FullName);
            }

            return returnList;
        }

        public List<Models.Provider> GetProviders()
        {
            var returnList = new List<Models.Provider>();

            var files = GetProvidersFiles();

            foreach (var current in files)
            {
                var item = new Models.Provider();

                item.ProviderClass = GetProviderClass(current);
                item.PathToProvider = current;
                item.ProviderName = item.ProviderClass.GetProviderName();

                if(item.ProviderClass != null) returnList.Add(item);
            }

            return returnList;
        }

        private PocoMachen.Integration.IProviderBinder GetProviderClass(string path)
        {
            var assembly = Assembly.LoadFrom(path);
            var type = typeof(PocoMachen.Integration.IProviderBinder);
            var classType = assembly.GetTypes().FirstOrDefault(p => type.IsAssignableFrom(p));
            if (classType == null) return null;

            return (PocoMachen.Integration.IProviderBinder)Activator.CreateInstance(classType);
        }

        public Models.Provider GetProvider(string providerName)
        {
            return GetProviders().FirstOrDefault(x => x.ProviderName.Equals(providerName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
