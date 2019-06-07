using CineTest.Entities;
using CineTest.Models;
using CineTest.Service;
using CineTest.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CineTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IQueryApiService _queryApiService;
        private readonly IWebClientService _webClientService;

        public HomeController(IConfiguration configuration, IQueryApiService queryApiService, IWebClientService webClientService)
        {
            _configuration = configuration;
            _queryApiService = queryApiService;
            _webClientService = webClientService;
        }

        public IActionResult Index()
        {
            Results results = _queryApiService.ListMovies(null, null);
            //Web.GetMovieList(ref results);
            return View(results);
        }

        [HttpPost]
        public IActionResult Index(long CurrentPage, int MoviesPerBatch, string Search, bool ResetPagination)
        {
            Results results = new Results() { CurrentPage = CurrentPage < 1 || ResetPagination ? 1 : CurrentPage, MoviesPerBatch = MoviesPerBatch < 1 ? 1 : MoviesPerBatch > 200 ? 200 : MoviesPerBatch, Search = Search };
            //Web.GetMovieList(ref results);
            return View(results);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}