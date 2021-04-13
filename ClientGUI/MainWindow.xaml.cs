using RestSharp;
using ServerInterfaceLib;
using System.ServiceModel;
using System.Windows;

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
            var registerControl = new UsernamePasswordUserControl("Enter your username and password to register in the database.", Register);

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
            var loginControl = new UsernamePasswordUserControl("Enter your username and password to log in.", Login);

            Window loginWindow = new Window
            {
                Title = "Register",
                Content = loginControl,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            loginWindow.Show();

        }
    }
}