using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for UsernamePasswordUserControl.xaml
    /// UserControl for inputting username and password and performing action based on these.
    /// </summary>
    public partial class UsernamePasswordUserControl : UserControl
    {
        //Action to perform upon login
        private readonly Action<string, string> _confirmAction;

        private readonly bool _closeOnConfirm;

        /// <summary>
        /// Constructor for UserControl.
        /// </summary>
        /// <param name="prompt">Prompt string to display before input</param>
        /// <param name="confirmAction">Function to perform with username and password upon confirmation</param>
        /// <param name="closeOnConfirm">Whether the current window should be closed upon pressing "confirm"</param>
        public UsernamePasswordUserControl(string prompt, Action<string, string> confirmAction, bool closeOnConfirm)
        {
            _confirmAction = confirmAction;
            _closeOnConfirm = closeOnConfirm;

            InitializeComponent();

            Prompt.Text = prompt;
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameInput.Text) && !string.IsNullOrEmpty(PasswordInput.Text))
            {
                //Run action in delegator to run with username & password data
                _confirmAction(UsernameInput.Text, PasswordInput.Text);

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
