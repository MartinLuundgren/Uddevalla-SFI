using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Models;

namespace Solution.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private SFI_DBEntities db = new SFI_DBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            //Test
            // Boomfunk Mc ROCKA MAJKROFÅN!!!!
            return View();
            
        }
        [HttpPost]
        public ActionResult Index(Segment segment)
        {
            db.Segments.Add(segment);
            db.SaveChanges();
            return View();
        }
    }
}