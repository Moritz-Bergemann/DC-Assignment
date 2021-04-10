using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServerInterfaceLib;

namespace Authenticator
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class AuthenticationServer : IAuthenticationServer
    {
        private readonly string accountsPath;
        private readonly string tokensPath;

        private readonly Random random;

        private AuthenticationServer(string accountsPath, string tokensPath)
        {
            this.accountsPath = accountsPath;
            this.tokensPath = tokensPath;
            this.random = new Random();

            //Create files if do not already exist
            using (StreamWriter w = File.AppendText(accountsPath)) ;
            using (StreamWriter w = File.AppendText(tokensPath)) ;
        }

        /// <summary>
        /// Registers a new user in the administration system
        /// </summary>
        /// <param name="name"></param> Username for registration
        /// <param name="password"></param> Password for registration
        /// <returns></returns> "successfully registered" if registration successful, error message otherwise
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Register(string name, string password)
        {
            string[] accountsFile = File.ReadAllLines(accountsPath);

            if (name.Contains(" ") || password.Contains(" "))
                return "Error: username/password cannot contain vertical bar ('|')";

            //See if user with matching username exists
            foreach (string line in accountsFile)
            {
                if (!line.Equals("")) //Skip empty lines
                {
                    string[] elements = line.Split('|');

                    if (elements.Length != 2)
                        throw new Exception("Broken File!!"); //TODO proper error handling

                    if (elements[0].Equals(name))
                    {
                        return "Error: user with name already registered";
                    }
                }
            }

            //Write new user to storage
            string formattedUserData = $"{name}|{password}";
            //TODO what if the fileIO messes up

            try
            {
                using (StreamWriter fileWriter = File.AppendText(accountsPath))
                {
                    fileWriter.Write(formattedUserData);
                    fileWriter.Close();
                }
            }
            catch (IOException io)
            {
                return "Error: Failed to save registration data";
            }


            return "successfully registered";
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int Login(string name, string password)
        {
            //Search accounts for username/password combo
            string[] accountsFile = File.ReadAllLines(accountsPath);

            bool match = false;
            foreach (string line in accountsFile)
            {
                string[] elements = line.Split(' ');

                if (elements.Length != 2)
                    throw new Exception("Broken File!!"); //TODO proper error handling

                if (elements[0].Equals(name) && elements[1].Equals(password))
                {
                    match = true;
                    break;
                }
            }

            int token = -1; //Token is -1 (indicating login failed) by default TODO get this checked

            if (match)
            {
                //Generate token (making sure it hasn't already been picked)
                string curTokensString = File.ReadAllText(tokensPath);

                bool verified = false;
                while (!verified)
                {
                    token = random.Next();
                    if (!curTokensString.Contains(token.ToString()))
                    {
                        verified = true;
                    }
                }

                //Write token to file
                using (StreamWriter tokenFileWriter = File.AppendText(tokensPath)) //TODO what happens if it cooks the fileIO 
                {
                    tokenFileWriter.WriteLine(token.ToString());
                } 
            }

            return token;
        }

        /// <summary>
        /// Validates if the input token integer has been validated against a user.
        /// </summary>
        /// <param name="token"></param> Integer that should have been validated against a user
        /// <returns></returns> "validated" if token was validated, "not validated" if it was not
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Validate(int token)
        {
            try
            {
                string curTokensString = File.ReadAllText(tokensPath);
                if (curTokensString.Contains(token.ToString()))
                {
                    return "validated";
                }
                else
                {
                    return "not validated";
                }
            }
            catch (IOException)
            {
                return "not validated";
            }
        }

        private static void Main(string[] args)
        {
            string accountsPath = "./accounts.txt";
            string tokensPath = "./tokens.txt";
            
            Console.WriteLine("Starting authentication server...");
            Console.WriteLine($"Using accounts path '{accountsPath}' and tokens path '{tokensPath}'");

            //Create service host 
            NetTcpBinding tcp = new NetTcpBinding();
            string url = "net.tcp://0.0.0.0:8101/AuthenticationProvider";

            //Bind service host to server
            //ServiceHost host = new ServiceHost(typeof(AuthenticationServer));
            ServiceHost host = new ServiceHost(new AuthenticationServer(accountsPath, tokensPath)); //Bind service host to existing instance, making it a singleton //TODO is this ok?
            host.AddServiceEndpoint(typeof(IAuthenticationServer), tcp, url);
            host.Open();

            Console.WriteLine("Server Online.");

            Console.ReadLine();
            host.Close();
        }
    }
}
