using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service_layer
{
    public class SLManager
    {
        private string SLRoot;
        private LoginData loginData;
        private readonly RestClient restClient;
        private string sessionID;
        public SLManager(string root, LoginData loginData)
        {
            this.SLRoot = root;
            this.loginData = loginData;
            restClient = new RestClient(this.SLRoot);
            Login();
        }

        public string GetSessionId()
        {
            return sessionID;
        }
        private void Login()
        {
            try
            {
                var request = new RestRequest("Login", Method.POST);

                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(loginData);

                var response = restClient.Execute(request);

                if (response.IsSuccessful)
                {
                    LoginResponse loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
                    sessionID = loginResponse.SessionId;
                    restClient.AddDefaultHeader("Authorization", $"Bearer {loginResponse.SessionId}");
                }
                else
                {
                    SLError error = JsonConvert.DeserializeObject<SLError>(response.Content);
                    throw new Exception(error.error.message.value);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Post(string resource, object data)
        {
            try
            {
                var request = new RestRequest($"{resource}", Method.POST);
                request.AddJsonBody(data);

                var response = restClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    SLError error = JsonConvert.DeserializeObject<SLError>(response.Content);
                    throw new Exception(error.error.message.value);
                }

                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class LoginData
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string SessionId { get; set; }
        public string Version { get; set; }
        public int SessionTimeOut { get; set; }
    }
    public class SLError
    {
        public Error error { get; set; }
    }

    public class Error 
    {
        public int code { get; set; }
        public ErrorMessage message { get; set; }
    }

    public class ErrorMessage
    {
        public string lang { get; set; }
        public string value { get; set; }
    }
}
