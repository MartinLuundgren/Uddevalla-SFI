using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solution.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
            //NS LOCALE
        }

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult Assignment()
        {
            return View();
        }
    }
}