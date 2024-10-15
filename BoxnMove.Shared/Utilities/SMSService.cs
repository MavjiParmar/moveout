using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Shared.Utilities
{
    public class SMSService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public SMSService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["SMSKey:ApiKey"];
        }

        public async Task<(bool IsSuccess, int Code)> SendOtpAsync(string phoneNumber)
        {
            var code = GenerateSMSCode();
            var data = new Dictionary<string, string>
{
{ "module", "TRANS_SMS" },
{ "apikey", _apiKey },
{ "to", phoneNumber },
{ "from", "MOVERR" },
{ "msg", GenerateMessage(code) }
};
            var content = new FormUrlEncodedContent(data);
            var response = await _httpClient.PostAsync("https://2factor.in/API/R1/", content);
            return (response.IsSuccessStatusCode, code);
        }

        string GenerateMessage(int code)
        {
            return $"{code} is the OTP to log into MOVER app by BOXNMOVE. 123456789";
        }
        int GenerateSMSCode()
        {
            try
            {
                return CodeGenerator.GenerateOTP();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception_SMSService_GenerateSMSCode_" + ex.Message);
                return 4312;
            }
        }
    }
}

