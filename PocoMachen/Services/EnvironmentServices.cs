using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.Services
{
    public class EnvironmentServices
    {
        public Dictionary<string, string> GetCommandLineArguments()
        {
            var returnList = new Dictionary<string,string>();

            string[] args = Environment.GetCommandLineArgs();

            foreach (var current in args)
            {
                string replaced = current.Replace(":=", "|");
                var split = replaced.Split('|');
                if (split.Count() > 1)
                {
                    returnList.Add(split[0].ToLower(),split[1]);
                }
            }

            return returnList;
        }
    }
}
