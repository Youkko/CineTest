using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CineTest.Entities
{
    [DataContract]
    public class Genre
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}