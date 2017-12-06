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
        //Pull fail comment 2
        public ActionResult Index()
        {
            var getSegments = (from s in db.Segments
                               orderby s.Name
                               select s).ToList();

            return View(getSegments);
        }

        public ActionResult Category(string segment)
        {
            int id = int.Parse(segment);
            var viewCategory = (from c in db.Categories
                                join s in db.Segments
                                on c.Segment_ID equals s.ID
                                where c.Segment_ID == id
                                select new JoinModelCategory {categories = c, segment = s }
                ).ToList();
            return View(viewCategory);
        }

        public ActionResult Assignment()
        {
            return View();
        }
    }
}