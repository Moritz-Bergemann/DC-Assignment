using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using APIClasses.Registry;
using Newtonsoft.Json;
using RestSharp;
using ServerInterfaceLib;

namespace ServicePublishingConsole
{
    class App
    {
        //User login token
        private int _token;
        private IAuthenticationServer _authServer;
        private RestClient _registryClient;

        public App(string authUrl, string registryUrl)
        {
            //Get the data server
            NetTcpBinding tcp = new NetTcpBinding();

            //Create connection factory for connection to auth server
            ChannelFactory<IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, authUrl);
            _authServer = serverChannelFactory.CreateChannel();

            //Setup registry server client
            _registryClient = new RestClient(registryUrl);

            _token = -1;
        }

        public void Run()
        {
            try
            {
                bool run = true;
                
                Console.WriteLine("Service publishing command line program.\n" +
                                  "Enter 'help' for command information.");

                while (run)
                {
                    try
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
                    catch (NullReferenceException)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Bad input.");
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
            Console.WriteLine("This application allows you to publish services using the networked service publisher.\n" +
                              "Command List:" +
                              "\nhelp - This output. \n" +
                              "\tregister - Register a new user with the authentication service. \n" +
                              "\tlogin - Get a new session token using your user credentials that allows you to interact with the publishing database. \n" +
                              "\tpublish - Publish a service to the registry server (Requires authorisation). \n" +
                              "\tunpublish - Remove a service from the registry server by API endpoint (Requires authorisation).");
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
            try
            {
                //Prepare registry data
                RegistryData data = new RegistryData();

                Console.Write("Service name: ");
                data.Name = Console.ReadLine();
                Console.Write("Service description: ");
                data.Description = Console.ReadLine();
                Console.Write("Service API endpoint: ");
                data.ApiEndpoint = Console.ReadLine();
                Console.Write("Number of operands: ");
                data.NumOperands = int.Parse(Console.ReadLine());
                Console.Write("Operand type: ");
                data.OperandType = Console.ReadLine();

                //Attempt publish on server
                RestRequest request = new RestRequest("api/publish");
                request.AddJsonBody(data);
                IRestResponse response = _registryClient.Post(request);
                PublishResult result = JsonConvert.DeserializeObject<PublishResult>(response.Content);

                Console.WriteLine($"Publish {(result.Success ? "succeeded" : "failed")} - {result.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Could not parse input. Aborted.");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Bad input. Aborted.");
            }
        }

        private void Unpublish()
        {
            try
            {
                //Prepare registry data
                EndpointData data = new EndpointData();

                Console.Write("API endpoint to unpublish: ");
                data.ApiEndpoint = Console.ReadLine();
                
                //Attempt publish on server
                RestRequest request = new RestRequest("api/unpublish");
                request.AddJsonBody(data);
                IRestResponse response = _registryClient.Post(request);
                PublishResult result = JsonConvert.DeserializeObject<PublishResult>(response.Content);

                Console.WriteLine($"Unpublish {(result.Success ? "succeeded" : "failed")} - {result.Message}");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Bad input. Aborted.");
            }
        }

        static void Main(string[] args)
        {
            //Intercept keyboard interrupt
            Console.CancelKeyPress += delegate
            {
                Console.WriteLine();
                Console.WriteLine("Keyboard Interrupt - Goodbye");
                Environment.Exit(0);
            };

            string authUrl = "net.tcp://localhost:8101/AuthenticationProvider";
            string registryUrl = "https://localhost:44330/";
            App app = new App(authUrl, registryUrl);

            app.Run();
        }
    }
}
