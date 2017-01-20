using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ContribeArbetsprov.Models
{
    public class BookstoreService : IBookstoreService
    {
        //Task that returns an IEnumerable of type IBook. Books come fron a Json.
        public async Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            var ListOfBooks = Task<IEnumerable<IBook>>.Factory.StartNew(() =>
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    var json = wc.DownloadString("http://www.contribe.se/arbetsprov-net/books.json");
                    var obj = JObject.Parse(json);// Parse the Json-string
                    var collection = obj as IEnumerable; //Turn the result of the parse in to a IEnumerable
                    List<IBook> Books = new List<IBook>();
                    foreach (var item in obj["books"])
                    {
                        string author = item["author"].ToString().ToUpper();
                        string title = item["title"].ToString().ToUpper();
                        if (author.Contains(searchString.ToUpper()) || title.Contains(searchString.ToUpper()))
                        {
                            IBook book = new Book((string)item["title"], (string)item["author"], (string)item["inStock"], (decimal)item["price"]);
                            Books.Add(book);
                        }
                    }
                    return Books;
                }
            });
            return await ListOfBooks;
        }

       
    }
}