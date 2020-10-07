using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace booksmanagement.Controllers
{
    public class BookBorrowController : Controller
    {
        // GET: BookBorrow
        public ActionResult Index()
        {
            return View();
        }
    }
}