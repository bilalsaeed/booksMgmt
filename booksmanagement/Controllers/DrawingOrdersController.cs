using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace booksmanagement.Controllers
{
    public class DrawingOrdersController : Controller
    {
        // GET: DrawingOrders
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }
    }
}