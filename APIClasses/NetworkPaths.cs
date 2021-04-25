using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses
{
    public class NetworkPaths
    {
        public static readonly string AuthenticatorUrl = "net.tcp://localhost:8101/AuthenticationProvider";
        public static readonly string RegistryUrl = "https://localhost:44330/";
        public static readonly string ServiceProviderUrl = "https://localhost:44365/";
    }
}
