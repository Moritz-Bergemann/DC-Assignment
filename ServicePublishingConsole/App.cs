using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using APIClasses;
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
                                break;
                            case "login":
                                Login();
                                break;
                            case "publish":
                                if (_token != -1)
                                {
                                    Publish();
                                }
                                else
                                {
                                    Console.WriteLine("Please log in before attempting to access the registry.");
                                }
                                break;
                            case "unpublish":
                                if (_token != -1)
                                {
                                    Unpublish();
                                }
                                else
                                {
                                    Console.WriteLine("Please log in before attempting to access the registry.");
                                }
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
                              "\thelp - This output. \n" +
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

            try
            {
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
            catch (CommunicationException c)
            {
                Console.WriteLine($"Failed to connect to authentication server - {c.Message}");
            }

        }

        private void Login()
        {
            Console.Write("Login Username: ");
            string username = Console.ReadLine();
            Console.Write("Login Password: ");
            string password = Console.ReadLine();

            Console.WriteLine("Attempting logon...");

            try
            {
                int token = _authServer.Login(username, password);

                if (token != -1)
                {
                    _token = token;
                    Console.WriteLine("Login successful. Authentication token changed.");
                }
                else //If login failed
                {
                    Console.WriteLine("Login failed. Authentication token has not been changed.");
                }
            }
            catch (CommunicationException c)
            {
                Console.WriteLine($"Failed to connect to authentication server - {c.Message}");
            }
        }

        private void Publish()
        {
            try
            {
                //Prepare registry data
                ServiceData data = new ServiceData();

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
                request.AddJsonBody(new PublishRequest(_token, data));
                IRestResponse response = _registryClient.Post(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    PublishResult result = JsonConvert.DeserializeObject<PublishResult>(response.Content);

                    if (result.Status.Equals("Accepted")) //First check if authentication succeeded
                    {
                        //Then check if publish itself succeeded
                        Console.WriteLine($"Publish {(result.Success ? "succeeded" : "failed")} - {result.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Registry access denied - {result.Reason}");
                    }
                }
                else
                {
                    Console.WriteLine($"Publish failed - server responded with error");
                }

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
                Console.Write("API endpoint to unpublish: ");
                string apiEndpoint = Console.ReadLine();

                //Attempt publish on server
                RestRequest request = new RestRequest("api/unpublish");
                request.AddJsonBody(new UnpublishRequest(_token, apiEndpoint));
                IRestResponse response = _registryClient.Post(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    PublishResult result = JsonConvert.DeserializeObject<PublishResult>(response.Content);

                    if (result.Status.Equals("Accepted")) //First check if authentication succeeded
                    {
                        //Then check if unpublish itself succeeded
                        Console.WriteLine($"Unpublish {(result.Success ? "succeeded" : "failed")} - {result.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Registry access denied - {result.Reason}");
                    }
                }
                else
                {
                    Console.WriteLine($"Publish failed - server responded with error");
                }

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

            string authUrl = NetworkPaths.AuthenticatorUrl;
            string registryUrl = NetworkPaths.RegistryUrl;
            App app = new App(authUrl, registryUrl);

            app.Run();
        }
    }
}
