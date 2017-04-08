using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

using RestClient;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Clairvoyant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServiceHost sh = null;
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
            sh = new ServiceHost(typeof(Service), new Uri(ConfigurationManager.AppSettings["HostServiceURL"]));
            bool openSucceeded = false;
            //TRY OPENINNG, IF FAILS THE HOST WILL BE ABORTED 
            try
            {
                ServiceEndpoint sep = sh.AddServiceEndpoint(typeof(Service), new WebHttpBinding(), "Hosting");
                sep.Behaviors.Add(new WebHttpBehavior());
                sh.Open();
                openSucceeded = true;
            }
            catch (Exception ex)
            {
                lblServiceStatus.Content = "Service host failed to open";
            }
            finally
            {
                if (!openSucceeded)
                    sh.Abort();                
            }

            if (sh.State == CommunicationState.Opened)
                lblServiceStatus.Content = "The Service is running";
            else
                lblServiceStatus.Content = "Server failed to open";                        
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            bool closeSucceed = false;
            try
            {
                if (sh != null)
                {
                    closeSucceed = true;
                    sh.Close();
                }   
            }
            catch
            {
                
            }
            finally
            {
                if (!closeSucceed)
                    sh.Abort();                
            }
        }
    }
}
