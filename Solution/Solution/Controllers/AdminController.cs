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
            //test1
            return View();
        }

        public ActionResult newSegment()
        {
            //TODO: Maybe use a View model? But this works 
            var segments = (from s in db.Segments
                            orderby s.Name
                            select s).ToList();
            ViewBag.segments = segments;
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
                    return RedirectToAction("newSegment", "Admin");
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
        [HttpGet]
        public ActionResult newAssignment()
        {
            return View();
        }
        [HttpPost]
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