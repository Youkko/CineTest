using Newtonsoft.Json;
using CineTest.Service.Interface;
using CineTest.Service.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;


namespace CineTest.Service
{
    public class WebClientService : IWebClientService
    {

        private readonly string _serviceUrl;

        public WebClientService(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        public T HttpRequest<T>(
            string controller,
            string action,
            object body,
            HttpResquestMethod method = HttpResquestMethod.GET,
            AuthorizationType authType = AuthorizationType.NoAuth,
            string token = "",
            int timeoutMin = 0)
        {
            return JsonConvert.DeserializeObject<T>(this.HttpRequest(controller, action, body, method, authType, token, timeoutMin));
        }
        public string HttpRequest(
           string controller,
           string action,
           object body,
           HttpResquestMethod method = HttpResquestMethod.GET,
           AuthorizationType authType = AuthorizationType.NoAuth,
           string token = "",
           int timeoutMin = 0)
        {
            var client = new HttpClient();
            HttpResponseMessage response = null;

            if (timeoutMin > 0)
            {
                client.Timeout = TimeSpan.FromMinutes(timeoutMin);
            }

            string url = string.Format(@"{0}/{1}/{2}", _serviceUrl, controller, action);
            string jsonBody = "";

            if (body != null)
            {
                jsonBody = JsonConvert.SerializeObject(body);
            }

            if (authType == AuthorizationType.BearerToken)
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                else
                {
                    throw new Exception("Token não informado");
                }
            }

            switch (method)
            {
                case HttpResquestMethod.GET:
                    response = client.GetAsync(url).Result;
                    break;
                case HttpResquestMethod.POST:
                    response = client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json")).Result;
                    break;
                case HttpResquestMethod.PUT:
                    response = client.PutAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json")).Result;
                    break;
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(String.Format("{0} - {1}", (int)response.StatusCode, response.ReasonPhrase));
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
