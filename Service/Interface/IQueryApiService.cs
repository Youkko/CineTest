using CineTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineTest.Service.Interface
{
    public interface IQueryApiService
    {
        Results ListMovies(int? moviesPerPage, int? currentPage);
        string RequestToken();
    }
}
