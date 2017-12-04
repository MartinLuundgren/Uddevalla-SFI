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
            //test12
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
            /*var categories = (from s in db.Categories
                              orderby s.Name
                              select s).ToList();
            ViewBag.categories = categories;*/

            //Hämta segmentnamn till dopdown i newCategories
            var segmentName = (from s in db.Segments
                              orderby s.Name
                              select s).ToList();
            ViewBag.segmentName = segmentName;

            //Joina segmentnamn och motsvarande kategorier för lista i newCategories
            var join = (from c in db.Categories
                        join s in db.Segments
                        on c.Segment_ID equals s.ID
                        select new JoinModel { categoryName = c.Name, segmentName = s.Name, categoryID = c.ID, segmentID=s.ID}).ToList();
            ViewBag.join = join;
            return View();
        }
        [HttpPost]
        public ActionResult newCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("newCategory", "Admin");
                }
                //TODO: Add exception, maybe custom error page? 
                catch (Exception)
                {
                    throw;
                }
            }
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