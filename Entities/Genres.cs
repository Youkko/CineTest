using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CineTest.Entities
{
    [DataContract]
    public class Genres
    {
        [DataMember(Name = "genres")]
        public List<Genre> Genre { get; set; }
    }
}