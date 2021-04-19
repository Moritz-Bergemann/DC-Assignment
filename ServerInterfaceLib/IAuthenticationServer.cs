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
        /// <summary>
        /// Registers a new user in the administration system
        /// </summary>
        /// <param name="name"></param> Username for registration
        /// <param name="password"></param> Password for registration
        /// <returns></returns> "successfully registered" if registration successful, error message otherwise
        [OperationContract]
        string Register(string name, string password);

        /// <summary>
        /// Generates a login token on the authentication server that can be validated, if the given name and password have been registered.
        /// </summary>
        /// <param name="name"></param> Username for login
        /// <param name="password"></param> Password for login
        /// <returns></returns> Registration token (positive integer) if login successful, '-1' otherwise
        [OperationContract]
        int Login(string name, string password);

        /// <summary>
        /// Validates if the input token integer has been validated against a user.
        /// </summary>
        /// <param name="token"></param> Integer that should have been validated against a user
        /// <returns></returns> "validated" if token was validated, "not validated" if it was not
        [OperationContract]
        string Validate(int token);
    }
}
