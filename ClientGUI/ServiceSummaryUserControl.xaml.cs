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
    /// Interaction logic for ServiceSummaryUserControl.xaml
    /// </summary>
    public partial class ServiceSummaryUserControl : UserControl
    {
        private Action<string, int> _testAction;
        private string _operandType;
        private int _numOperands;

        public ServiceSummaryUserControl(Action<string, int> testAction, int numOperands, string operandType)
        {
            _testAction = testAction;
            _numOperands = numOperands;
            _operandType = operandType;
            InitializeComponent();
        }

        private void Test_Button_Click(object sender, RoutedEventArgs e)
        {
            _testAction(_operandType, _numOperands);
        }
    }
}