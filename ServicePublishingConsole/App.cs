using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using ServerInterfaceLib;

namespace ServicePublishingConsole
{
    class App
    {
        //User login token
        private int _token;
        private IAuthenticationServer _authServer;

        public App(string url)
        {
            //Get the data server
            NetTcpBinding tcp = new NetTcpBinding();

            //Create connection factory for connection to server
            ChannelFactory<ServerInterfaceLib.IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, url);
            _authServer = serverChannelFactory.CreateChannel();

            _token = -1;
        }

        public void Run()
        {
            try
            {
                bool run = true;

                string introText = "Service publishing command line program.\n" +
                                   "Enter 'help' for command information.";
                Console.WriteLine(introText);

                while (run)
                {
                    Console.Write(">");
                    string[] userInputElements = Console.ReadLine().Split(' ');
                    switch (userInputElements[0])
                    {
                        case "help":
                            Help();
                            break;
                        case "register":
                            Register();
                            //new ArraySegment<string>(userInputElements, 1, userInputElements.Length - 1).Array //TODO remove this
                            break;
                        case "login":
                            Login();
                            //new ArraySegment<string>(userInputElements, 1, userInputElements.Length - 1).Array //TODO remove this
                            break;
                        case "publish":
                            Publish();
                            break;
                        case "unpublish":
                            Unpublish();
                            break;
                        case "exit":
                            run = false;
                            break;
                        default:
                            Console.WriteLine($"Unknown option '{userInputElements[0]}' - enter 'help' for help");
                            break;
                    }
                }

                Console.WriteLine("Goodbye!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Fatal Error: " + e);
            }
        }

        private static void Help()
        {
            string helpString =
                "This application allows you to publish services using the networked service publisher.\n" +
                "Command List:" +
                "\nhelp - This output. \n" +
                "\tregister - Register a new user with the authentication service. \n" +
                "\tlogin - Get a new session token using your user credentials that allows you to interact with the publishing database. \n" +
                "\tpublish - TBA \n" + //TODO
                "\tunpublish - TBA"; //TODO
            Console.WriteLine(helpString);
        }

        private void Register()
        {
            //Get details
            Console.Write("Registration Username: ");
            string username = Console.ReadLine();
            Console.Write("Registration Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Attempting registration...");

            //Register at remote server
            string response = _authServer.Register(username, password);

            if (response.Equals("successfully registered")) //If successfully registered
            {
                Console.WriteLine("Registration successful!");
            }
            else
            {
                Console.WriteLine($"Registration failed. ({response})");
            }
        }

        private void Login()
        {
            Console.Write("Login Username: ");
            string username = Console.ReadLine();
            Console.Write("Login Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Attempting logon...");

            int token = _authServer.Login(username, password);

            if (token != -1)
            {
                _token = token;
                Console.WriteLine("Login successful. Authentication token changed.");
                Console.WriteLine($"DEBUG: New token is '{_token}'");
            }
            else //If login failed
            {
                Console.WriteLine("Login failed. Authentication token has not been changed.");
            }
        }

        private void Publish()
        {
            Console.WriteLine("TBA"); //TODO
        }

        private void Unpublish()
        {
            Console.WriteLine("TBA"); //TODO
        }

        static void Main(string[] args)
        {
            string url = "net.tcp://localhost:8101/AuthenticationProvider";
            App app = new App(url);

            app.Run();
        }
    }
}
