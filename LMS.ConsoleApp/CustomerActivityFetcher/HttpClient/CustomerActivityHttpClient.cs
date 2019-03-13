using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace CustomerActivityFetcher.HttpClient
{
    public class CustomerActivityHttpClient : System.Net.Http.HttpClient,  ICustomerActivityHttpClient
    {
        private string _url;
        private Uri _uri;
        private string _action;
        private string _mediaType;

        private static readonly string SoapRequestString1 = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://comparenow/schemas/Services/MotorActivity/20120701"">
                        <soapenv:Body>
                            <ns:GetActivity><ns:customerActivityId>";
        private static readonly string SoapRequestString2 = $@"</ns:customerActivityId></ns:GetActivity>
                        </soapenv:Body>
                    </soapenv:Envelope>";

        private string _soapRequestString;

        private string Url
        {
            get => _url; 
            set => _url = value;
        }

        private string Action
        {
            get => _action;
            set => _action = value;
        }
        private Uri RequestUri
        {
            get => _uri;
            set => _uri = value;
        }

        private string MediaType
        {
            get => _mediaType;
            set => _mediaType = value;
        }
        public CustomerActivityHttpClient()
        {
            // TODO Set & Get from API Config - UserStory #91689
            Url = "http://utsvc.inspopusa.lan:57006/Comparenow_MAS_v1.0/MotorActivityService.svc";
            RequestUri = new Uri(Url);
            Action = "\"http://comparenow/schemas/Services/MotorActivity/20120701/IMotorActivity/GetActivity\"";
            DefaultRequestHeaders.Add("SOAPAction", Action);
            MediaType = "text/xml";
        }

        public StringContent RequestContent(Guid customerActivityGuid)
        {
            _soapRequestString = $"{SoapRequestString1}{customerActivityGuid}{SoapRequestString2}";
            return new StringContent(_soapRequestString, Encoding.UTF8, MediaType);
        }

        public async Task<string> GetCustomerActivity(Guid customerActivityGuid)
        {
            string customerActivity = String.Empty;
            try
            {
                HttpResponseMessage response = null;
                HttpStatusCode returnCode = HttpStatusCode.OK;
                response = await base.PostAsync(RequestUri, RequestContent(customerActivityGuid));
                returnCode = response.StatusCode;
                if (returnCode == HttpStatusCode.OK)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    customerActivity = HttpUtility.HtmlDecode(responseString);
                    Console.WriteLine($"CustomerActivity for Guid: {customerActivity}");
                    Console.WriteLine("The End.  Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    // Log Error
                }
            }
            catch (Exception e)
            {
                // Log exception
                throw;
            }
            return customerActivity;

        }
    }
}
//string soap = $@"<?xml version=""1.0"" encoding=""utf-8""?>
//        <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://comparenow/schemas/Services/MotorActivity/20120701"">
//            <soapenv:Body>
//                <ns:GetActivity><ns:customerActivityId>{customerActivityGuid}</ns:customerActivityId></ns:GetActivity>
//            </soapenv:Body>
//        </soapenv:Envelope>";
