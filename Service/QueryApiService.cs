using Newtonsoft.Json;
using CineTest.Service.Interface;
using CineTest.Service.Enums;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using CineTest.Entities;
using System.Collections.Generic;

namespace CineTest.Service
{
    public class QueryApiService : IQueryApiService
    {

        private readonly IWebClientService _webClientService;
        private readonly string _apiKey;

        public QueryApiService(IWebClientService webClientService, string apiKey)
        {
            _webClientService = webClientService;
            _apiKey = apiKey;
        }

        public Results ListMovies(int? moviesPerPage, int? currentPage)
        {
            string token = RequestToken();
            RootObject root;
            root = _webClientService.HttpRequest<RootObject>(
                        "Movies",
                        $"List?moviesPerPage={moviesPerPage}&currentPage={currentPage}",
                        null,
                        HttpResquestMethod.GET,
                        AuthorizationType.BearerToken,
                        token
                    );

            if (root.error)
            {
                throw new Exception(root.message.ToString());
            }
            else
            {
                return JsonConvert.DeserializeObject<Results>(root.message.ToString());
            }
        }

        public string RequestToken()
        {
            ApiToken token = _webClientService.HttpRequest<ApiToken>(
                "Login",
                "",
                new
                {
                    Key = _apiKey
                },
                HttpResquestMethod.POST
            );

            if (token.authenticated)
            {
                return token.token;
            }
            else
            {
                throw new Exception(token.message);
            }
        }
    }
}
