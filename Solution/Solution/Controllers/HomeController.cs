using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solution.Controllers
{
    public class HomeController : Controller
    {
        private SFI_DBEntities db = new SFI_DBEntities();
        // GET: Home
        //Pull fail comment
        public ActionResult Index()
        {
            var getSegments = (from s in db.Segments
                               orderby s.Name
                               select s).ToList();

            return View(getSegments);
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