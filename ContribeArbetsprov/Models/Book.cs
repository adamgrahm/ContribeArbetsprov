using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContribeArbetsprov.Models
{
    public class Book:IBook
    {
        public Book(string Title, string Author, string InStock, decimal Price)
        {
            this.Title = Title;
            this.Author = Author;
            this.InStock = InStock;
            this.Price = Price;
        }
        public string Title
        {
            get;
        }
        public string Author
        {
            get;
        }
        public decimal Price
        {
            get;
        }
        public string InStock
        {
            get;
        }
    }
}