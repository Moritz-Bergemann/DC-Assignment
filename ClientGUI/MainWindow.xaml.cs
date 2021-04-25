using APIClasses.Math;
using APIClasses.Registry;
using APIClasses.Security;
using Newtonsoft.Json;
using RestSharp;
using ServerInterfaceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using APIClasses;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAuthenticationServer _authServer;
        private RestClient _registryClient;

        //Authentication memory
        private int _loginToken;

        //Service test call memory
        private string _apiEndpoint = null;
        private int _numOperands = -1;
        private string _operandType = null;

        public MainWindow()
        {
            InitializeComponent();

            string authUrl = NetworkPaths.AuthenticatorUrl;
            string registryUrl = NetworkPaths.RegistryUrl;
            string serviceUrl = NetworkPaths.ServiceProviderUrl;

            _loginToken = -1;
            
            //Get the registry server
            _registryClient = new RestClient(registryUrl);

            //Create connection factory for connection to auth server
            NetTcpBinding tcp = new NetTcpBinding();
            ChannelFactory<IAuthenticationServer> serverChannelFactory = new ChannelFactory<IAuthenticationServer>(tcp, authUrl);
            _authServer = serverChannelFactory.CreateChannel();
        }
        
        private async Task Login(string username, string password)
        {
            int token;
            try
            {
                //Attempt login
                token = await Task.Run(() => _authServer.Login(username, password));
            }
            catch (CommunicationException)
            {
                MessageBox.Show("The client failed to establish a connection with the authentication service.", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _loginToken = token;

            //Show result in display box
            string registerResultText =
                token != -1 ? "Login Successful!" : "Login failed.";
            MessageBox.Show(registerResultText, "Login result", MessageBoxButton.OK);


            //Show result in login status
            LoginStatus.Text = token != -1 ? "Logged in." : "Not currently logged in.";
        }

        private async Task Register(string username, string password)
        {
            //Attempt login
            string result;
            try
            {
                result = await Task.Run(() => _authServer.Register(username, password));
            }
            catch (CommunicationException)
            {
                MessageBox.Show("The client failed to establish a connection with the authentication service.", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Show result in display box
            string registerResultText =
                result.Equals("successfully registered") ? "Register Successful!" : "Register failed.";
            MessageBox.Show(registerResultText, "Registration result", MessageBoxButton.OK);
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

        /// <summary>
        /// Fill the services list with the given list of services
        /// </summary>
        /// <param name="services">List of services to use for filling</param>
        private void FillServicesList(List<ServiceData> services)
        {
            //Add all found services to list of services in form of ServiceSummaryUserControl
            List<UIElement> controlsList = new List<UIElement>();
            if (services.Count > 0)
            {
                services.ForEach(registryData => controlsList.Add(new ServiceSummaryUserControl(PrepareServiceTest, registryData)));
            }
            else
            {
                TextBlock newText = new TextBlock();
                newText.Text = "No services have been found in the registry.";
                controlsList.Add(newText);
            }
            ServicesItemsControl.ItemsSource = controlsList;
        }

        private void PrepareServiceTest(ServiceData serviceData)
        {
            //Check service data is valid for testing
            if (!Formats.AllowedOperandTypes.Any(s => s.Equals(serviceData.OperandType))) //If service is not of allowed operand type
            {
                MessageBox.Show($"This service cannot be tested as the given operand type '{serviceData.OperandType}' is not permitted.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (serviceData.NumOperands < 1)
            {
                MessageBox.Show($"This service cannot be tested as the given number of operands '{serviceData.NumOperands}' is invalid.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool validUrl = Uri.TryCreate(serviceData.ApiEndpoint, UriKind.Absolute, out Uri _);
            if (!validUrl)
            {
                MessageBox.Show($"This service cannot be tested as the given API endpoint '{serviceData.ApiEndpoint}' is invalid.", "Cannot test", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Set up fields for calling API service
            _apiEndpoint = serviceData.ApiEndpoint;
            _operandType = serviceData.OperandType;
            _numOperands = serviceData.NumOperands;

            //Show service fields & description
            ServiceName.Text = serviceData.Name;
            ServiceDescription.Text = serviceData.Description;
            OperandType.Text = serviceData.OperandType;

            //Show input boxes for service
            List<TextBox> inputBoxes = new List<TextBox>();
            for (int ii = 0; ii < serviceData.NumOperands; ii++)
            {
                TextBox newBox = new TextBox();
                newBox.Name = $"ServiceInput{ii + 1}";
                inputBoxes.Add(newBox);
            }
            ServiceInputsItemsControl.ItemsSource = inputBoxes;
        }

        private async void Run_Service_Button_Click(object sender, RoutedEventArgs e)
        {
            //Check an API to test has actually been selected
            if (_apiEndpoint == null)
            {
                MessageBox.Show("Please select a service before testing.", "Select a service", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Construct IntegerMathRequest for service
            IntegerMathRequest input = new IntegerMathRequest(_loginToken, new List<int>());

            foreach (UIElement element in ServiceInputsItemsControl.Items)
            {
                if (element is TextBox box)
                {
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
                    }
                }
            }

            //Sanity check
            if (input.Values.Count != _numOperands)
                throw new ArgumentException("Number of service operands retrieved and required do not match");

            //Create request to API endpoint
            RestRequest request = new RestRequest(_apiEndpoint);
            request.AddJsonBody(input);

            //Create REST client for the API endpoint
            Uri uri = new Uri(_apiEndpoint);
            string uriLeft = uri.GetLeftPart(UriPartial.Authority);
            RestClient apiClient = new RestClient(uriLeft);

            //Show loading bar
            TestServiceProgressBar.Visibility = Visibility.Visible;
            //Make async request
            IRestResponse response = await AsyncRest.AsyncPost(request, apiClient);
            //Hide loading bar
            TestServiceProgressBar.Visibility = Visibility.Collapsed;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("The client failed to establish a connection with the given service.", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Display result at bottom
            MathResponse result = JsonConvert.DeserializeObject<MathResponse>(response.Content);

            //Check if authentication succeeded
            if (result.Status.Equals("Accepted"))
            {
                //Display result if successful and error message otherwise
                ServiceResult.Text = result.Success ? result.Result : result.Message;
            }
            else
            {
                ServiceResult.Text = $"Authentication denied - {result.Reason}";
            }
        }

        private async void Search_Service_Button_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;

            //Verify authentication data
            if (_loginToken == -1)
            {
                MessageBox.Show("Please log in before attempting to make a request to the server.", "Please login", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Prepare request for registry search result
            RestRequest request = new RestRequest("api/search");
            SearchRequest searchRequest = new SearchRequest(_loginToken, query);
            request.AddJsonBody(searchRequest);

            //Show loading bar
            ShowServicesProgressBar.Visibility = Visibility.Visible;
            //Make async request
            IRestResponse response = await AsyncRest.AsyncPost(request, _registryClient);
            //Hide loading bar again
            ShowServicesProgressBar.Visibility = Visibility.Collapsed;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("The client failed to establish a connection with the registry service.", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SearchResult result = JsonConvert.DeserializeObject<SearchResult>(response.Content);

            //Abort if service did not return successfully
            if (result.Status.Equals("Denied"))
            {
                MessageBox.Show($"The server request was rejected. Reason: {result.Reason}", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Fill services list with retrieved list
            FillServicesList(result.Values);
        }

        private async void Show_All_Services_Button_Click(object sender, RoutedEventArgs e)
        {
            //Verify authentication data
            if (_loginToken == -1)
            {
                MessageBox.Show("Please log in before attempting to make a request to the server.", "Please login", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //Make request for registry search result
            RestRequest request = new RestRequest("api/all");
            request.AddJsonBody(new SecureRequest(_loginToken));

            //Show loading bar
            ShowServicesProgressBar.Visibility = Visibility.Visible;
            //Make async request
            IRestResponse response = await AsyncRest.AsyncPost(request, _registryClient);
            //Hide loading bar again
            ShowServicesProgressBar.Visibility = Visibility.Collapsed;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("The client failed to establish a connection with the registry service.", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SearchResult result = JsonConvert.DeserializeObject<SearchResult>(response.Content);

            //Abort if service did not return successfully
            if (result.Status.Equals("Denied"))
            {
                MessageBox.Show($"The server request was rejected. Reason: {result.Reason}", "Request failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Fill services list with retrieved list
            FillServicesList(result.Values);
        }
    }
}