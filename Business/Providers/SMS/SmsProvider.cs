using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Business.Providers.SmsProvider
{
    public interface ISmsProvider
    {
        Task<SmsResponse> SendSms(string phoneNumber, string message);

        Task<SmsResponse> CheckBalance();
    }
    public class SmsConfig
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string Url { get; set; }
        public string Mask { get; set; }
    }

    public class SmsResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class SmsProvider : ISmsProvider
    {
        private readonly SmsConfig _smsConfig;
        private readonly IConfiguration _configuration;

        public SmsProvider(IConfiguration configuration)
        {
            _smsConfig = new SmsConfig();
            _configuration = configuration;
        }

        public async Task<SmsResponse> CheckBalance()
        {
            try
            {
                _configuration.GetSection("SmsConfig").Bind(_smsConfig);

                var client = new RestClient(_smsConfig.Url);
                var request = new RestRequest("checkcredits", Method.GET);
                request.AddQueryParameter("username", _smsConfig.Username);
                request.AddQueryParameter("password", _smsConfig.Password);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var result = new SmsResponse()
                    {
                        Status = true,
                        Message = response.Content
                    };
                    return result;
                }
                else
                {
                    var result = new SmsResponse()
                    {
                        Status = false,
                        Message = response.ErrorMessage
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SmsResponse> SendSms(string phoneNumber, string message)
        {
            try
            {
                _configuration.GetSection("SmsConfig").Bind(_smsConfig);

                phoneNumber = phoneNumber.Replace(" ", string.Empty);

                if (phoneNumber.StartsWith("+234"))
                {
                    phoneNumber = phoneNumber.Replace("+234", "0");
                }

                if (phoneNumber.StartsWith("234"))
                {
                    phoneNumber = phoneNumber.Replace("234", "0");
                }

                phoneNumber = phoneNumber.Substring(1).Insert(0, "+234");

                var client = new RestClient(_smsConfig.Url);
                var request = new RestRequest("sendsms", Method.GET);
                request.AddQueryParameter("username", _smsConfig.Username);
                request.AddQueryParameter("password", _smsConfig.Password);
                request.AddQueryParameter("from", _smsConfig.Sender);
                request.AddQueryParameter("to", phoneNumber);
                request.AddQueryParameter("text", message);
                request.AddQueryParameter("dlr-mask", _smsConfig.Mask);
                var response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var result = new SmsResponse()
                    {
                        Status = true,
                        Message = response.Content
                    };
                    return result;
                }
                else
                {
                    var result = new SmsResponse()
                    {
                        Status = false,
                        Message = response.ErrorMessage
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
