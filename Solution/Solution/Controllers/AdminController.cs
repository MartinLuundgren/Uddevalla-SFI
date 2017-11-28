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
            
            return View();
        }
        public ActionResult newSegment()
        {
       
            return View();
        }
        public ActionResult newCategory()
        {
            return View();
        }
        public ActionResult newAssignment()
        {
            return View();
        }
    }
}