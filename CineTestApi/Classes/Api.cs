using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineTestApi.Interface;
using CineTest.Entities;
using RestSharp;
using Newtonsoft.Json;

namespace CineTestApi
{
    public class Api : IApi
    {
        private readonly IConfiguration _configuration;
        private readonly string _fullUrl, _apiKey;
        private List<KeyValuePair<string, string>> parameters;
        public Api(IConfiguration configuration)
        {
            _configuration = configuration;
            _fullUrl = $"{_configuration.GetSection("TMDb").GetSection("Api").Value}/{_configuration.GetSection("TMDb").GetSection("Version").Value}";
            _apiKey = $"{_configuration.GetSection("TMDb").GetSection("Key").Value}";
        }

        public Results ListMovies(int moviesPerPage, int currentPage)
        {
            Results           results         = new Results();
            RestRequest       request         = new RestRequest(Method.GET);
            List<Result>      result          = new List<Result>(), 
                              tempResult      = new List<Result>();
            string            url             = $"{_fullUrl}/discover/movie?",
                              searchQuery     = results.Search;
            long              currentRealPage = 1;

            SetParameters(currentRealPage);
            parameters.Add(new KeyValuePair<string, string>("sort_by", "primary_release_date.desc"));
            parameters.Add(new KeyValuePair<string, string>("include_video", "false"));

            FindResults(moviesPerPage, currentPage, out results, ref request, out result, url, ref currentRealPage);
            return results;
        }

        public Results SearchMovies(string query, int moviesPerPage, int currentPage)
        {
            Results      results         = new Results();
            RestRequest  request         = new RestRequest(Method.GET);
            List<Result> result          = new List<Result>(),
                         tempResult      = new List<Result>();
            string       url             = $"{_fullUrl}/search/movie?";
            long         currentRealPage = 1;

            SetParameters(currentRealPage);
            parameters.Add(new KeyValuePair<string, string>("query", query));

            FindResults(moviesPerPage, currentPage, out results, ref request, out result, url, ref currentRealPage, query);
            return results;
        }

        private void SetParameters(long currentPage)
        {
            parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("api_key", $"{_apiKey}"));
            parameters.Add(new KeyValuePair<string, string>("language", "en-US"));
            parameters.Add(new KeyValuePair<string, string>("include_adult", "false"));
            parameters.Add(new KeyValuePair<string, string>("page", $"{currentPage}"));
        }

        private void FindResults(int moviesPerPage, int currentPage, out Results results, ref RestRequest request, out List<Result> result, string url, ref long currentRealPage, string query = "")
        {
            QueryClient(new RestClient($"{url}{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}"), ref request, out results);

            result = results.Result;

            if (currentRealPage > 1)
            {
                int qttOfThisPage = (int)((currentRealPage / (currentPage - 1)) * 20);
                if (qttOfThisPage > moviesPerPage)
                {
                    result.RemoveRange(0, (qttOfThisPage - moviesPerPage));
                }
            }

            while (result.Count < moviesPerPage)
            {
                results = ReQuery(ref request, ref parameters, url, ref currentRealPage);
                if (results.Result.Count == 0) break;
                result.AddRange(results.Result);
            }

            if (result.Count > moviesPerPage)
            {
                int diff = result.Count - moviesPerPage;
                result.RemoveRange(moviesPerPage - 1, diff);
            }

            results.Result = result;
            results.CurrentPage = currentPage;
            results.MoviesPerBatch = moviesPerPage;
            GetConfiguration(ref results);
            if (results.Genres == null) GetGenres(ref results);
            CalculatePages(ref results);
            results.Search = query;
        }

        private Results ReQuery(ref RestRequest request, ref List<KeyValuePair<string,string>> parameters, string url, ref long currentRealPage)
        {
            Results results;
            var pageParam = parameters.First(i => i.Key == "page");
            var indexPageParam = parameters.IndexOf(pageParam);

            if (indexPageParam > -1) parameters[indexPageParam] = new KeyValuePair<string, string>("page", $"{++currentRealPage}");
            QueryClient(new RestClient($"{url}{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}"), ref request, out results);
            return results;
        }

        private void CalculatePages(ref Results results)
        {
            results.TotalPages = results.Total_Results == 0 ? 1 : (long)Math.Ceiling((double)(results.Total_Results / results.MoviesPerBatch));
        }

        private long CalculateRealPage(ref Results results)
        {
            long currentRegistry = results.CurrentPage <= 1 ? 1 : ((results.CurrentPage - 1)* results.MoviesPerBatch);
            return currentRegistry <= 1 ? 1 : (long)Math.Floor((double)(currentRegistry / 20));
        }

        private void GetConfiguration(ref Results results)
        {
            ApiConfiguration result = new ApiConfiguration();
            RestRequest request = new RestRequest(Method.GET);
            string url = $"{_fullUrl}/configuration?";
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("api_key", $"{_apiKey}"));
            QueryClient(new RestClient($"{url}{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}"), ref request, out result);
            results.Config = result;
        }

        private void GetGenres(ref Results results)
        {
            Genres result;
            RestRequest request = new RestRequest(Method.GET);
            string url = $"{_fullUrl}/genre/movie/list?";
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("api_key", $"{_apiKey}"));
            QueryClient(new RestClient($"{url}{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}"), ref request, out result);
            results.Genres = result;
        }

        private void QueryClient<T>(RestClient client, ref RestRequest request, out T results)
        {
            IRestResponse response = client.Execute(request);
            results = JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
