using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Cooking.Infrastructure.Sms.Contracts;
using Cooking.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Cooking.Infrastructure.Sms
{
    public class SMSIRMessageService : IBaseSmsService
    {
        private readonly SmsSettings _smsSettings;
        private string GetTokenURL;
        private string LineNumber;
        private string PatternSmsURL;
        private string SecretKey;
        private string SMSURL;
        private string System;
        private string UserApiKey;

        public SMSIRMessageService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
            InitilizeOptions();
        }


        public string SendParameterizedMessage(ParameterizedSmsMessageDto dto)
        {
            var svcModel = new
            {
                Mobile = dto.Receivers[0],
                TemplateId = dto.Template.ToString("D"),
                ParameterArray = dto.Parameters.Select(p => new
                {
                    Parameter = p.Key,
                    ParameterValue = p.Value?.ToString() ?? string.Empty
                })
            };

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, PatternSmsURL)
            {
                Content = new StringContent(JsonConvert.SerializeObject(svcModel))
            };
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var token = GetToken().Result;
            request.Headers.Add("x-sms-ir-secure-token", token);

            var response = new HttpResponseMessage();
            try
            {
                response = client.SendAsync(request).Result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var result = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(result);
        }

        public void InitilizeOptions()
        {
            SMSURL = _smsSettings.SMSURL;
            PatternSmsURL = _smsSettings.PatternSmsURL;
            GetTokenURL = _smsSettings.GetTokenURL;
            UserApiKey = _smsSettings.UserApiKey;
            SecretKey = _smsSettings.SecretKey;
            System = _smsSettings.System;
            LineNumber = _smsSettings.LineNumber;
        }

        private Task<string> GetToken()
        {
            return Task.Run(() =>
            {
                var svcModel = new
                {
                    UserApiKey,
                    SecretKey,
                    System
                };

                var json = JsonConvert.SerializeObject(svcModel);
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, GetTokenURL);
                request.Content = new StringContent(json, Encoding.UTF8);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                var result = client.SendAsync(request).Result;
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var ret = JsonConvert.DeserializeObject<ApiResult.TokenResult>(result.Content.ReadAsStringAsync()
                        .Result);
                    if (ret.IsSuccessful) return ret.TokenKey;
                }

                return (string) null;
            });
        }
    }

    public abstract class ApiResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }

        public class TokenResult : ApiResult
        {
            public string TokenKey { get; set; }
        }

        public class VerificationCodeResult : ApiResult
        {
            public string VerificationCodeId { get; set; }
        }

        public class MessageSendResult : ApiResult
        {
            public List<MessageSentId> Ids { get; set; }
            public string BatchKey { get; set; }

            public class MessageSentId
            {
                public long ID { get; set; }
                public string MobileNo { get; set; }
            }
        }
    }
}