using CineTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTestApi.Interface
{
    public interface IApi
    {
        Results ListMovies(int moviesPerPage, int currentPage);
        Results SearchMovies(string query, int moviesPerPage, int currentPage);
    }
}
