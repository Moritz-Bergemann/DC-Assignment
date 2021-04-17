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
using APIClasses.Registry;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for ServiceSummaryUserControl.xaml
    /// </summary>
    public partial class ServiceSummaryUserControl : UserControl
    {
        private readonly Action<RegistryData> _testAction;
        private readonly RegistryData _serviceData;

        public ServiceSummaryUserControl(Action<RegistryData> testAction, RegistryData serviceData)
        {
            _testAction = testAction;
            _serviceData = serviceData;
            InitializeComponent();
        }

        private void Test_Button_Click(object sender, RoutedEventArgs e)
        {
            _testAction(_serviceData);
        }
    }
}