﻿using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using ServerInterfaceLib;
using System.ServiceModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using APIClasses.Math;
using APIClasses.Registry;
using Newtonsoft.Json;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] allowedOperandTypes = { "integer" }; //NOTE - currently only integer supported, but more can be added here

        private IAuthenticationServer _authServer;
        private RestClient _registryClient;
        private RestClient _serviceClient;

        //Authentication memory
        private int _loginToken;

        //Service test call memory
        private string _apiEndpoint = null;
        private int _numOperands = -1;
        private string _operandType = null;

        public MainWindow()
        {
            InitializeComponent();

            string authUrl = "net.tcp://localhost:8101/AuthenticationProvider";
            string registryUrl = "https://localhost:44330/";
            string serviceUrl = "https://localhost:44365/";

            _loginToken = -1;
            
            //Get the registry server
            _registryClient = new RestClient(registryUrl);

            //Get the service provider server
            _serviceClient = new RestClient(serviceUrl);

            //Create connection factory for connection to auth server
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, authUrl);
            _authServer = serverChannelFactory.CreateChannel();
        }
        
        //TODO these probably should go somewhere else
        private void Login(string username, string password)
        {
            //TODO async display loading

            //Attempt login
            int token = _authServer.Login(username, password);

            _loginToken = token;

            //Show result in login status
            LoginStatus.Text = token != -1 ? "Logged in." : "Not currently logged in.";
        }

        private void Register(string username, string password)
        {
            //TODO async display loading

            //Attempt login
            string result = _authServer.Register(username, password);

            //Show result in login status
            string registerResultText =
                result.Equals("successfully registered") ? "Register Successful!" : "Register failed.";
            MessageBox.Show(registerResultText, "Registration result", MessageBoxButton.OK); //TODO
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create username/password UserControl for registering (giving it register function to execute)
            var registerControl = new UsernamePasswordUserControl("Enter your username and password to register in the database.", Register, true);

            Window registerWindow = new Window
            {
                Title = "Register",
                Content = registerControl,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            registerWindow.Show();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            //Create username/password UserControl for login (giving it login function to execute)
            var loginControl = new UsernamePasswordUserControl("Enter your username and password to log in.", Login, true);

            Window loginWindow = new Window
            {
                Title = "Register",
                Content = loginControl,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            loginWindow.Show();
        }

        private void FillServicesList(List<RegistryData> services)
        {
            //Add all found services to list of services in form of ServiceSummaryUserControl
            List<UIElement> controlsList = new List<UIElement>();
            if (services.Count > 0)
            {
                services.ForEach(registryData => controlsList.Add(new ServiceSummaryUserControl(null, registryData)));
            }
            else
            {
                TextBlock newText = new TextBlock();
                newText.Text = "No services have been found in the registry.";
                controlsList.Add(newText);
            }
            ServicesItemsControl.ItemsSource = controlsList;
        }

        private void PrepareServiceTest(RegistryData serviceData)
        {
            //Check service data is valid for testing
            if (!allowedOperandTypes.Any(s => s.Equals(serviceData.OperandType))) //If 
            {
                MessageBox.Show($"This service cannot be tested as the given operand type '{serviceData.OperandType}' is not permitted.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (serviceData.NumOperands < 1)
            {
                MessageBox.Show($"This service cannot be tested as the given number of operands '{serviceData.NumOperands}' is invalid.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Uri uriResult;
            bool validUrl = Uri.TryCreate(serviceData.ApiEndpoint, UriKind.Absolute, out uriResult);
            if (!validUrl)
            {
                MessageBox.Show($"This service cannot be tested as the given API endpoint '{serviceData.ApiEndpoint}' is invalid.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Set up fields for calling API service
            _apiEndpoint = serviceData.ApiEndpoint;
            _operandType = serviceData.OperandType; //TODO maybe check the type here?
            _numOperands = serviceData.NumOperands;

            List<TextBox> inputBoxes = new List<TextBox>();

            for (int ii = 0; ii < serviceData.NumOperands; ii++)
            {
                TextBox newBox = new TextBox();
                newBox.Name = $"ServiceInput{ii + 1}";
                inputBoxes.Add(newBox);
            }

            ServiceInputsItemsControl.ItemsSource = inputBoxes;
        }

        private void Run_Service_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check an API to test has actually been selected
            if (_apiEndpoint == null)
            {
                MessageBox.Show("Please select a service before testing.", "Select a service", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Construct MathInput for service
            MathInput input = new MathInput();

            foreach (UIElement element in ServiceInputsItemsControl.Items)
            {
                if (element is TextBox)
                {
                    TextBox box = (TextBox) element;

                    switch (_operandType) //NOTE - add more statements to switch to support more operand types
                    {
                        case "integer":
                            if (int.TryParse(box.Text, out var parsedInput))
                            {
                                input.Values.Add(parsedInput);
                            }
                            else
                            {
                                //Show error and exit
                                MessageBox.Show("Could not start service - please enter valid integers for all parameters.", "Integer parameters needed", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                            break;
                        default:
                            //Should only be thrown if the operand type was somehow not checked before assignment (fatal error)
                            throw new ArgumentException("Attempting API test with invalid argument type");
                            break;
                    }
                }
            }

            //Sanity check
            if (input.Values.Count != _numOperands)
            {
                throw new ArgumentException("Number of service operands retrieved and required do not match");
            }

            //Make request to API endpoint
            RestRequest request = new RestRequest(_apiEndpoint);
            request.AddJsonBody(input);

            IRestResponse response = _serviceClient.Post(request);
        }


        private void Search_Service_Button_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;

            //Make request for registry search result
            RestRequest request = new RestRequest("api/search");
            SearchData searchData = new SearchData(query);
            request.AddJsonBody(searchData);
            IRestResponse response = _registryClient.Post(request);
            List<RegistryData> result = JsonConvert.DeserializeObject<List<RegistryData>>(response.Content);

            //Fill services list with retrieved list
            FillServicesList(result);
        }

        private void Show_All_Services_Button_Click(object sender, RoutedEventArgs e)
        {
            //Make request for registry search result
            RestRequest request = new RestRequest("api/all");
            IRestResponse response = _registryClient.Get(request);
            List<RegistryData> result = JsonConvert.DeserializeObject<List<RegistryData>>(response.Content);

            //Fill services list with retrieved list
            FillServicesList(result);
        }
    }
}