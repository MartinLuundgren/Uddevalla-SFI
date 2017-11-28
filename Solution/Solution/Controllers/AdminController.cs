using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Models;
using System.IO;

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
            //TODO: add functionality
            var segments = (from s in db.Segments
                            orderby s.Name
                            select s).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult newSegment(Segment segment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Segments.Add(segment);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
                //TODO: Add exception, maybe custom error page? 
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
        }
        public ActionResult newCategory()
        {
            return View();
        }
        public ActionResult newAssignment(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                ViewBag.Message = "File uploaded successfully.";
            }
            return View();
        }
    }
}