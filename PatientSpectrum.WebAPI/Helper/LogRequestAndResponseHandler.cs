using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using System.Configuration;

namespace PatientSpectrum.WebAPI.Helper
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        public object Formatting { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isLogEnabled = false;
            // log request body
            string requestBody = await request.Content.ReadAsStringAsync();

            bool.TryParse(ConfigurationManager.AppSettings["captureAllRequestResponses"], out isLogEnabled);

            if (isLogEnabled)
            {
                Log.Information("REQUEST Body : " + requestBody);
                Log.Information("REQUEST Header : " + SerializeHeaders(request.Headers.ToList()));
            }

            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);

            if (isLogEnabled)
            {
                if (result.Content != null)
                {
                    // once response body is ready, log itzx+
                    var responseBody = await result.Content.ReadAsStringAsync();
                    Log.Information("RESPONSE Body : " + responseBody);
                    Log.Information("RESPONSE Headers : " + SerializeHeaders(result.Content.Headers));
                }
            }

            return result;
        }

        private string SerializeHeaders(List<KeyValuePair<string, IEnumerable<string>>> list)
        {
            var dict = new Dictionary<string, string>();



            foreach (var item in list)
            {
                var inner = item.Value;
                var header = new StringBuilder();
                foreach (var subitem in inner)
                {
                    var strVal = subitem.ToList();
                    foreach (var insubitem in strVal)
                    {
                        header.Append(insubitem);
                    }
                }
                dict.Add(item.Key, header.ToString());
            }

            return JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
        }

        private string SerializeHeaders(HttpContentHeaders headers)
        {
            var dict = new Dictionary<string, string>();

            foreach (var item in headers.ToList())
            {
                if (item.Value != null)
                {
                    var header = String.Empty;
                    foreach (var value in item.Value)
                    {
                        header += value + " ";
                    }

                    // Trim the trailing space and add item to the dictionary
                    header = header.TrimEnd(" ".ToCharArray());
                    dict.Add(item.Key, header);
                }
            }

            return JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
        }
    }
}