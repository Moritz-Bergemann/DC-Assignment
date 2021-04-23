using ServerInterfaceLib;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;

namespace Authenticator
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class AuthenticationServer : IAuthenticationServer
    {
        private readonly string _accountsPath;
        private readonly string _tokensPath;

        private readonly Random _random;
        private Timer _tokenClearer;

        private AuthenticationServer(string accountsPath, string tokensPath)
        {
            _accountsPath = accountsPath;
            _tokensPath = tokensPath;
            _random = new Random();
            _tokenClearer = null;

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
            string[] accountsFile = File.ReadAllLines(_accountsPath);

            if (name.Contains(" ") || password.Contains(" "))
                return "Error: username/password cannot contain vertical bar ('|')";

            //See if user with matching username exists
            foreach (string line in accountsFile)
            {
                if (!line.Equals("")) //Skip empty lines
                {
                    string[] elements = line.Split('|');

                    if (elements.Length != 2)
                        throw new FormatException("Invalid format for registry file");

                    if (elements[0].Equals(name))
                    {
                        return "Error: user with name already registered";
                    }
                }
            }

            //Write new user to storage
            string formattedUserData = $"{name}|{password}";

            try
            {
                using (StreamWriter fileWriter = File.AppendText(_accountsPath))
                {
                    fileWriter.WriteLine(formattedUserData);
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
            string[] accountsFile = File.ReadAllLines(_accountsPath);

            bool match = false;
            foreach (string line in accountsFile)
            {
                string[] elements = line.Split('|');

                if (elements.Length != 2)
                    throw new FormatException("Invalid format for registry file");

                if (elements[0].Equals(name) && elements[1].Equals(password))
                {
                    match = true;
                    break;
                }
            }

            int token = -1; //Token is -1 (indicating login failed) by default

            if (match)
            {
                //Generate token (making sure it hasn't already been picked)
                string curTokensString = File.ReadAllText(_tokensPath);

                bool verified = false;
                while (!verified)
                {
                    token = _random.Next();
                    if (!curTokensString.Contains(token.ToString()))
                    {
                        verified = true;
                    }
                }

                //Write token to file
                using (StreamWriter tokenFileWriter = File.AppendText(_tokensPath))
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
                string curTokensString = File.ReadAllText(_tokensPath);
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

        private void InitTokenClearer(int interval)
        {
            _tokenClearer = new Timer(ClearTokens, null, interval, interval);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ClearTokens(object state)
        {
            Console.WriteLine("Clearing tokens...");
            //Overwrite tokens file to completely wipe it
            File.WriteAllText(_tokensPath, "") ;
        }

        private static void Main(string[] args)
        {
            string accountsPath = "C:/dc-assignment-1/accounts.txt";
            string tokensPath = "C:/dc-assignment-1/tokens.txt";
            
            Console.WriteLine("Starting authentication server...");
            Console.WriteLine($"Using accounts path '{accountsPath}' and tokens path '{tokensPath}'");

            //Create service host 
            NetTcpBinding tcp = new NetTcpBinding();
            string url = "net.tcp://0.0.0.0:8101/AuthenticationProvider";

            AuthenticationServer server = new AuthenticationServer(accountsPath, tokensPath);

            //Bind service host to server
            ServiceHost host = new ServiceHost(server); //Bind service host to existing instance, making it a singleton
            host.AddServiceEndpoint(typeof(IAuthenticationServer), tcp, url);
            host.Open();

            Console.WriteLine("Server Online.");

            //Read in user input for interval at which tokens should be cleared
            bool check = false;
            int interval = -1;
            while (!check)
            {
                Console.Write("Please enter the token clear interval (in seconds): ");
                //https://stackoverflow.com/questions/6169288/execute-specified-function-every-x-seconds

                try
                {
                    interval = int.Parse(Console.ReadLine());

                    if (interval >= 1)
                    {
                        check = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a value greater than 1.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Please enter a valid integer.");
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Please enter a valid integer.");
                }
            }

            //Create timer to wipe file at given interval
            server.InitTokenClearer(interval * 1000);

            Console.WriteLine($"Wiping at interval of {interval} seconds.");

            Console.ReadLine();
            host.Close();
        }
    }
}
