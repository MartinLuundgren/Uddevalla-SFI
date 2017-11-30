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

        public ActionResult Category(string segment)
        {
            int id = int.Parse(segment);
            Segment temp = (Segment)TempData["segment"];
            var category = (from c in db.Categories
                            where c.Segment_ID == id
                            select c).ToList();
            return View(category);
        }

        public ActionResult Assignment()
        {
            return View();
        }

        //Fixa loginsida för admin
       /* public ActionResult Login()
        {
            return RedirectToAction("Index","Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                var details = (from userList in db.Logins
                               where userList.Name == login.Name && userList.Password == login.Password
                               select new
                               {
                                   userList.ID,
                                   userList.Name
                               }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["ID"] = details.FirstOrDefault().ID;
                    Session["Name"] = details.FirstOrDefault().Name;
                    return RedirectToAction("Loggedin");
                }
            }
            else
            {
                ModelState.AddModelError("","Felaktiga uppgifter");
            }
            return View(login);
        }

        public ActionResult Loggedin()
        {
            return RedirectToAction("Index","Admin");
        }*/
    }
}