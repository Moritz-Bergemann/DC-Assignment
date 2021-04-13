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
    /// </summary>
    public partial class UsernamePasswordUserControl : UserControl
    {
        //Action to perform upon login
        private readonly Action<string, string> _confirmAction;

        public UsernamePasswordUserControl(string prompt, Action<string, string> confirmAction)
        {
            _confirmAction = confirmAction;

            InitializeComponent();

            Prompt.Text = prompt;
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(UsernameInput.Text) && !string.IsNullOrEmpty(PasswordInput.Text))
            {
                //Run action in delegator to run with username & password data
                _confirmAction(UsernameInput.Text, PasswordInput.Text);
            }
            else
            {
                EmptyContentWarning.Visibility = Visibility.Visible;
            }
        }
    }
}
