using Newtonsoft.Json;
using SecureElementReader.App.Enpoints;
using SecureElementReader.App.Interfaces;
using SecureElementReader.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SecureElementReader.App.Proxies
{
    public class TaxCoreApiProxy : ITaxCoreApiProxy
    {
        private string commonName { get; set; }
        private string apiUrl { get; set; }
        private string uniqueIdentifier { get; set; }

        public async Task<List<CommandsStatusResult>> CommandStatusUpdate(List<CommandsStatusResult> commandResult)
        {
            List<CommandsStatusResult> result = new List<CommandsStatusResult>();

            try
            {
                using (var handler = new HttpClientHandler())
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        GetClientAndHandler(handler, client);

                        foreach (var item in commandResult)
                        {
                            var httpContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                            var response = await client.PostAsync($"{EndpointUrls.NotifyCommandProcessed}", httpContent);

                            string responseBody = await response.Content.ReadAsStringAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                result.Add(new CommandsStatusResult { CommandId = item.CommandId, Success = false });
                            }
                            else
                            {
                                result.Add(new CommandsStatusResult { CommandId = item.CommandId, Success = true });
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception)
            {
                return result;
            }
        }

        public async Task<List<Command>>? GetPendingCommands()
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        GetClientAndHandler(handler, client);                        
                        var response = await client.PostAsync($"{EndpointUrls.GetPendingCommands}?serialNumber={uniqueIdentifier}", null);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            return null;
                        }

                        var commands = JsonConvert.DeserializeObject<List<Command>>(responseBody);
                        return commands.Where(s=>s.Type == Enums.CommandsType.ForwardSecureElementDirective 
                                || s.Type == Enums.CommandsType.ForwardProofOfAudit).ToList();
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SendInternalData(SecureElementAuditRequest request)
        {
            try
            {
                using (var handler = new HttpClientHandler())
                {
                    using (HttpClient client = new HttpClient(handler))
                    {
                        GetClientAndHandler(handler, client);
                        var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                        var response = await client.PostAsync($"{EndpointUrls.SubmitInternalData}", httpContent);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            return false;
                        }

                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Configure(string uniqueIdentifier, string commonName, string apiUrl)
        {
            this.commonName = commonName;
            this.uniqueIdentifier = uniqueIdentifier;
            this.apiUrl = apiUrl.EndsWith('/') ? apiUrl : apiUrl + "/";
        }

        private void GetClientAndHandler(out HttpClientHandler handler, out HttpClient client)
        {
            handler = CreateWebRequestHandler();
            client = new HttpClient(handler);

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
        }

        private void GetClientAndHandler(HttpClientHandler handler, HttpClient client)
        {
            var cert = GetClientCertificate();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ClientCertificates.Add(cert);

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.ConnectionClose = false;
        }

        private HttpClientHandler CreateWebRequestHandler()
        {
            var handler = new HttpClientHandler();
            var cert = GetClientCertificate();

            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ClientCertificates.Add(cert);
            return handler;
        }

        private X509Certificate2 GetClientCertificate()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
            return store.Certificates.Find(X509FindType.FindBySubjectName, commonName, false)[0];
        }
    }
}
