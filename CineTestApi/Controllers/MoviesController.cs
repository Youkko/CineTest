using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineTest.Entities;
using CineTestApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineTestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IApi _api;
        public MoviesController(IApi api)
        {
            _api = api;
        }

        // GET api/movies/list
        // GET api/movies/list?moviesPerPage=1&currentPage=1
        // GET api/movies/list?currentPage=5
        [Authorize("Bearer")]
        public Results List(int? moviesPerPage, int? currentPage)
        {
            int movPerPage = 110,
                curPage = 1;
            if (moviesPerPage.HasValue) movPerPage = moviesPerPage.Value;
            if (currentPage.HasValue) curPage = currentPage.Value;
            return _api.ListMovies(movPerPage, curPage);
        }

        // GET api/movies/search?name=casper
        // GET api/movies/search?name=casper&moviesPerPage=1&currentPage=1
        // GET api/movies/search?name=casper&currentPage=5
        [Authorize("Bearer")]
        public Results Search(string name, int? moviesPerPage, int? currentPage)
        {
            int movPerPage = 110,
                curPage    = 1;
            if (moviesPerPage.HasValue) movPerPage = moviesPerPage.Value;
            if (currentPage.HasValue) curPage = currentPage.Value;
            return _api.SearchMovies(name, movPerPage, curPage);
        }
    }
}