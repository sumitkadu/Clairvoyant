using System.ServiceModel;
using System.ServiceModel.Web;

namespace Clairvoyant
{
    [ServiceContract]
    public class Service
    {
        [WebInvoke(UriTemplate = "/posterror", Method = "POST", RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        public string PostMessage(string input)
        {
            return input;
        }        
    }
}
