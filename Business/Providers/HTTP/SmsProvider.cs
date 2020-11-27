using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Providers.HTTP
{
    public class SmsProvider : ISmsProvider
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        private readonly Uri baseUrl;

        public SmsProvider(IHttpClientFactory httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            baseUrl = new Uri(_configuration["SmsConfig:BaseUrl"]);
        }

        //sendsinglesms
        //sendbulksms

        public async Task<HttpResponseMessage> GetApiResponse(HttpMethod method, string subsidiaryUrl, object model = null)
        {
            var apiUrl = new Uri(baseUrl, subsidiaryUrl);
            var request = new HttpRequestMessage(method, apiUrl); //get method fully and partly post method

            request.Headers.Add("X-App-AppID", _configuration["SmsConfig:Username"]);//expecting name and authorization
            request.Headers.Add("X-App-ApiKey", _configuration["SmsConfig:Password"]);

            if (model != null)
            {
                var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                request.Content = new StringContent(json, Encoding.UTF8, "application/json"); //complete post metho
            }

            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset-utf-8");

            var response = await client.SendAsync(request);
            client.Dispose();
            return response;
        }
    }
}
