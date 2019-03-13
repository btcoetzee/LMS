using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CustomerActivityFetcher.HttpClient;

namespace CustomerActivityFetcher
{
    class Program
    {
        const string  Url = "http://utsvc.inspopusa.lan:57006/Comparenow_MAS_v1.0/MotorActivityService.svc";
        const string  Action = "http://comparenow/schemas/Services/MotorActivity/20120701/IMotorActivity/GetActivity";
        private static readonly ICustomerActivityHttpClient customerActivityHttpClient = new CustomerActivityHttpClient();
        private static Guid[] _customerActivityGuidList = new Guid[]
            {
                Guid.Parse("0D73D0C1-8100-41DD-AF9E-C9298CB0A967"),
                Guid.Parse("3A4080BC-9868-4A0B-AE85-E1714C76835F"),

            };
        static void Main(string[] args)
        {

            HttpClientToMotorActivityService _httpClientToMotorActivityService = new HttpClientToMotorActivityService(customerActivityHttpClient);

            foreach (var customerActivityGuid in _customerActivityGuidList)
            {

                _httpClientToMotorActivityService.GetCustomerActivity(customerActivityGuid).Wait();
            }

            //WebRequestMethod(_customerActivityGuid.ToString());

        }

        #region HttpWebRequest Method

        private static void WebRequestMethod(string customerActivityGuid)
        {
            
            Console.WriteLine($"Guid : {customerActivityGuid}");

            string soap = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://comparenow/schemas/Services/MotorActivity/20120701"">
                    <soapenv:Header/>
                        <soapenv:Body>
                            <ns:GetActivity><ns:customerActivityId>{customerActivityGuid}</ns:customerActivityId></ns:GetActivity>
                        </soapenv:Body>
                    </soapenv:Envelope>";

            // Create the XML Document using Soap Envelope
            System.Xml.XmlDocument soapEnvelopeXml = CreateXmlLDocument(soap);

            // Create the WebRequest using the Url and Action
            HttpWebRequest webRequest = CreateWebRequest(Url, Action);

            // Add XML Document to webRequest
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // Async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. 
            asyncResult.AsyncWaitHandle.WaitOne();

            // Get response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.WriteLine(soapResult);
            }

            Console.WriteLine("The End.  Press any key to continue...");
            Console.ReadKey();
        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static System.Xml.XmlDocument CreateXmlLDocument(string soap)
        {
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.LoadXml(soap);
            return xmlDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(System.Xml.XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
        #endregion
    }

    public class HttpClientToMotorActivityService
    {
        private readonly ICustomerActivityHttpClient _customerActivityHttpClient;
        public HttpClientToMotorActivityService(ICustomerActivityHttpClient customerActivityHttpClient)
        {
            _customerActivityHttpClient = customerActivityHttpClient;
        }

        public async Task<string> GetCustomerActivity(Guid customerActivityGuid)
        {
            string customerActivity = String.Empty;

            customerActivity = await _customerActivityHttpClient.GetCustomerActivity(customerActivityGuid);
            
            return customerActivity;
        }


    }

}
