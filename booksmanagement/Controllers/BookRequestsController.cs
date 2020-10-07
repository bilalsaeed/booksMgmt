using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace booksmanagement.Controllers
{
    public class BookRequestsController : Controller
    {
        // GET: BookRequests
        public ActionResult Index()
        {
            return View();
        }
    }
}