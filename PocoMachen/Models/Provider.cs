using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PocoMachen.Models
{
    public class Provider
    {
        public string ProviderName { get; set; }
        public string PathToProvider { get; set; }
        public PocoMachen.Integration.IProviderBinder ProviderClass { get; set; }
    }
}
