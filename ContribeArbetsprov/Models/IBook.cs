using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContribeArbetsprov.Models
{
    public interface IBook
    {
        string Title { get; }
        string Author { get; }
        decimal Price { get; }
        string InStock { get; }

    }
}