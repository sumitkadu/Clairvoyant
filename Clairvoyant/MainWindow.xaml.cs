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

using RestClient;

namespace Clairvoyant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            //txtPayload.Text = "{\n  \"ISO_DATE\": \"2016-12-30 16:27:54,435\", \n  \"MESSAGE\": \"FxRestfulController: USDUSD not found: ccy1 and ccy2 should not be the same FxRestfulController.java 208 \", \n  \"STATUS\": \"OK\"\n}\n";
            if (!string.IsNullOrWhiteSpace(txtPayload.Text))
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    LogInformation information = RestClient.RestClient.GetData(txtPayload.Text, System.Configuration.ConfigurationManager.AppSettings["RestServiceURL"]);

                    lblStatus.Text = information.Status;
                    lblLogClass.Text = information.LogClass;
                    lblDate.Text = information.ISO_DATE;
                    lblMessage.Text = information.ORGMSG;

                    dataGrid.ItemsSource = information.relatedIncident;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            }
            else
            {
                MessageBox.Show("Please provide input payload", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
