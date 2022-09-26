using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Proxies
{
    public class TaxCoreApiProxy : ITaxCoreApiProxy
    {

        private static string? CommonName;
        private static string? ApiUrl;

        public bool SendInternalData(SecureElementAuditRequest request, string commonName, string apiUrl)
        {

            CommonName = commonName;
            ApiUrl = apiUrl;

            var httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            HttpClient client;
            HttpClientHandler handler;

            GetClientAndHandler(out handler, out client);

            var response = client.PostAsync($"/api/SecureElements/Audit", httpContent).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                var invoiceResponse = jsonString.Result;
                return true;
            }
            else
            {
                return false;
            }
        }

        static void GetClientAndHandler(out HttpClientHandler handler, out HttpClient client)
        {
            handler = CreateWebRequestHandler();
            client = new HttpClient(handler);

            client.BaseAddress = new Uri(ApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
        }

        static HttpClientHandler CreateWebRequestHandler()
        {
            var handler = new HttpClientHandler();
            var cert = GetClientCertificate();

            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ClientCertificates.Add(cert);
            return handler;
        }

        static X509Certificate2 GetClientCertificate()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            return store.Certificates.Find(X509FindType.FindBySubjectName, CommonName, false)[0];
        }
    }
}
