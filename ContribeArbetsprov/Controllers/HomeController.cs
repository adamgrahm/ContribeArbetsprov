using ContribeArbetsprov.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ContribeArbetsprov.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }
        //Calls on the bookstoreservice that returns a list of books!
        public async Task<ActionResult> Index(string searchString = "")
        {

            BookstoreService BookStore = new BookstoreService();
            var result = await BookStore.GetBooksAsync(searchString);
            return View(result);
        }

        //Adding items to the cart
        public ActionResult AddToCart(string Title, string Author, decimal Price, string InStock)
        {
            if (Title != null && Author != null && Price != 0 && InStock != null)
            {
                IBook book = new Book(Title, Author, InStock, Price);
                List<IBook> BooksInCart;
                if (Session["cart"] == null)
                {
                    BooksInCart = new List<IBook>();
                }
                else
                {
                    BooksInCart = (List<IBook>)Session["cart"];
                }
                if (book.InStock == "0")
                {
                    ViewBag.ErrorMessage = "Sorry this item is out of stock";
                }
                BooksInCart.Add(book);
                Session["cart"] = BooksInCart;

                //Checking if user tries to buy more books than there are in stock.
                List<IBook> books = (List<IBook>)Session["cart"];
                for (int i = 0; i < books.Count(); i++)
                {
                    IBook NewBook = books[i];
                    int inCart = books.Where(x => x.Author == book.Author)
                                        .Where(x => x.Title == book.Title)
                                        .Where(x => x.Price == book.Price)
                                        .Count();

                    int inStock;
                    int.TryParse(book.InStock, out inStock);
                    if (inCart > inStock)
                    {
                        var difference = inCart - inStock;
                        for (int o = 0; o < difference; o++)
                        {
                            books.Remove(book);
                            
                                TempData["errorMessage"] = "You can't buy anymore of this product!";
                            
                        }
                    }
                }
                
                Session["cart"] = books;
            }
            return RedirectToAction("GoToCart");
        }


        public ActionResult GoToCart()
        {
            ViewBag.ErrorMessage = TempData["errorMessage"];
            List<IBook> CartItems = (List<IBook>)Session["cart"];
                return View(CartItems);

        }

        public ActionResult RemoveFromCart(string Author, string Title, decimal Price)
        {
            List <IBook> Cart = (List<IBook>)Session["cart"];
            var findItem = Cart.FirstOrDefault(x => x.Author == Author && x.Title == Title && x.Price == Price);
            Cart.Remove(findItem);
            Session["cart"] = Cart;
            List<IBook> NewCart = (List<IBook>)Session["cart"];
            return RedirectToAction("GoToCart",NewCart);
        }

        public ActionResult PlaceOrder()
        {
            List<IBook> orderedBooks = (List<IBook>)Session["cart"];
            if (orderedBooks.Count() > 0)
            {
                return View(orderedBooks);
            }
            ViewBag.ErrorMessage = "You need to put something in the cart in order to place an order";
            return View();
            
        }

        public ActionResult GoBackToStore()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}