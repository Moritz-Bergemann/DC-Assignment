using System.Collections.Generic;
using RestSharp;
using ServerInterfaceLib;
using System.ServiceModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using APIClasses.Registry;
using Newtonsoft.Json;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IAuthenticationServer _authServer;
        private RestClient _registryClient;

        private int _loginToken;

        public MainWindow()
        {
            InitializeComponent();

            string authUrl = "net.tcp://localhost:8101/AuthenticationProvider";
            string registryUrl = "https://localhost:44330/";

            _loginToken = -1;

            //Get the data server
            NetTcpBinding tcp = new NetTcpBinding();

            //Create connection factory for connection to auth server
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

        private void FillServicesList(List<RegistryData> services)
        {
            List<UserControl> controlsList = new List<UserControl>();
            if (services.Count == 0)
            {
                services.ForEach(registryData => controlsList.Add(new ServiceSummaryUserControl(null, registryData)));
            }
            else
            {
                //TODO show something here
            }
            ServicesItemsControl.ItemsSource = controlsList;
        }
    }
}