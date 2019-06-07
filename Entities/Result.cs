using System;
using System.Collections.Generic;

namespace CineTest.Entities
{
    public class Result
    {
        public long Vote_Count { get; set; }
        public long Id { get; set; }
        public bool Video { get; set; }
        public long Vote_Average { get; set; }
        public string Title { get; set; }
        public double Popularity { get; set; }
        public string Poster_Path { get; set; }
        public string Original_Language { get; set; }
        public string Original_Title { get; set; }
        public List<int> Genre_Ids { get; set; }
        public string Backdrop_Path { get; set; }
        public bool Adult { get; set; }
        public string Overview { get; set; }
        public DateTime? Release_Date { get; set; }
    }
}