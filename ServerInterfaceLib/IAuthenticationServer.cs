using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterfaceLib
{
    [ServiceContract]
    public interface IAuthenticationServer
    {
        [OperationContract]
        string Register(string name, string password);

        [OperationContract]
        int Login(string name, string password);

        [OperationContract]
        string Validate(int token);            
    }
}
