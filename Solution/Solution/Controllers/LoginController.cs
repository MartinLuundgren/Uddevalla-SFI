using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Models;
using System.IO;

namespace Solution.Controllers
{
    public class LoginController : Controller
    {
        private SFI_DBEntities db = new SFI_DBEntities();
        // GET: Login
        public ActionResult Index(Login login)
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
                    return RedirectToAction("Index","Admin");
                }
            }
            else
            {
                ModelState.AddModelError("", "Felaktiga uppgifter");
            }
            return View(login);
        }
    }
}