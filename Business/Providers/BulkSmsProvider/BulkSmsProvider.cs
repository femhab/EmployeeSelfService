using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Business.Providers
{
    public class BulkSmsProvider: IBulkSmsProvider
    {
        private readonly SmsConfig _smsConfig;
        private readonly IConfiguration _configuration;


        public BulkSmsProvider(IOptions<SmsConfig> smsConfig, IConfiguration configuration)
        {
            _smsConfig = smsConfig.Value;
            _configuration = configuration;
            _configuration.GetSection("SmsConfig").Bind(_smsConfig);
        }

        public async Task SendSms(string recipient, string message)
        {

            string requestUrl = $"?api_token={_smsConfig.ApiToken}&from={_smsConfig.From}&to={recipient}&body={message}";
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_smsConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(requestUrl);
            }

        }
    }
}
