using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for UsernamePasswordUserControl.xaml
    /// UserControl for inputting username and password and performing action based on these.
    /// </summary>
    public partial class UsernamePasswordUserControl : UserControl
    {
        //Action to perform upon login
        private readonly Func<string, string, Task> _confirmTask;

        private readonly bool _closeOnConfirm;

        /// <summary>
        /// Constructor for UserControl.
        /// </summary>
        /// <param name="prompt">Prompt string to display before input</param>
        /// <param name="confirmTask">Function to perform with username and password upon confirmation</param>
        /// <param name="closeOnConfirm">Whether the current window should be closed upon pressing "confirm"</param>
        public UsernamePasswordUserControl(string prompt, Func<string, string, Task> confirmTask, bool closeOnConfirm)
        {
            _confirmTask = confirmTask;
            _closeOnConfirm = closeOnConfirm;

            InitializeComponent();

            Prompt.Text = prompt;
        }

        private async void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameInput.Text) && !string.IsNullOrEmpty(PasswordInput.Text))
            {
                //Show loading bar
                UsernamePasswordProgressBar.Visibility = Visibility.Visible;

                //Run action in delegator to run with username & password data
                await _confirmTask(UsernameInput.Text, PasswordInput.Text);

                if (_closeOnConfirm)
                {
                    Window.GetWindow(this)?.Close();
                }
            }
            else
            {
                EmptyContentWarning.Visibility = Visibility.Visible;
            }
        }
    }
}
