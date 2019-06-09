using System;
using System.Collections.Generic;
using System.Text;

namespace CineTest.Entities
{
    public class Itens<T> where T : IObject
    {
        public List<T> item { get; set; }
    }
}
