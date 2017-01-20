using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ContribeArbetsprov.Models
{
    public interface IBookstoreService
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);
    }
}