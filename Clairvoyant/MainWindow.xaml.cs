using System;
using System.Windows;
using System.Windows.Input;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RemoteWcfService.RestServiceClient client = new RemoteWcfService.RestServiceClient();
            txtRestOutput.Text = client.GetData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
