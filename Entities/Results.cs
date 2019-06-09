using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CineTest.Entities
{
    [DataContract]
    public class Results
    {
        private int _moviesPerBatch;
        public ApiConfiguration Config { get; set; }
        public Genres Genres { get; set; }
        public long CurrentPage { get; set; }
        public long TotalPages { get; set; }
        [DataMember(Name = "total_results")]
        public long Total_Results { get; set; }
        [DataMember(Name = "total_pages")]
        public long Total_Pages { get; set; }
        [DataMember(Name = "results")]
        public List<Result> Result { get; set; }
        public long DisplayInfoFrom { get; set; }
        public string Search { get; set; }
        public int MoviesPerBatch { get { return _moviesPerBatch; } set { _moviesPerBatch = (value > 0) ? value : 1; } }
        public Results()
        {
            MoviesPerBatch = 105;
            DisplayInfoFrom = 1;
        }
    }
}