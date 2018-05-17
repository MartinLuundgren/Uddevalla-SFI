using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Solution.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Data.Entity;

namespace Solution.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        //DB connection
        private SfiDbEntities db = new SfiDbEntities();
        
        // GET: Admin
        public ActionResult Index()
        {
            //Check to see if the user is logged in
            if (Session["Name"] != null)
            {
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult Index(StartPageMovy startPageMovie)
        {
            if (Session["Name"] != null)
            {
                var id = (from l in db.StartPageMovies
                            orderby l.Id descending
                            select l.Id).FirstOrDefault();
                StartPageMovy s = db.StartPageMovies.Find(id);
                s.URL = Regex.Replace(startPageMovie.URL, @"watch\W\w\W", "embed/");
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        //New segment
        public ActionResult newSegment()
        {
            if (Session["Name"] != null)
            {
                var segments = (from s in db.Segments
                                orderby s.Name
                                select s).ToList();
                ViewBag.segments = segments;
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult newSegment(Segment segment)
        {
            if (Session["Name"] != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //TODO: RegEx for URL 
                        //segment.URL = Regex.Replace(segment.URL, @"watch\W\w\W", "embed/");
                        db.Segments.Add(segment);
                        db.SaveChanges();
                        return RedirectToAction("newSegment", "Admin");
                    } 
                    catch (Exception)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
            return View();
        }

        public ActionResult EditSegment(int? id)
        {
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Segment segment = db.Segments.Find(id);
                if (segment == null)
                {
                    return HttpNotFound();
                }
                return View(segment);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSegment([Bind(Include = "ID,URL,Name")] Segment segment)
        {
            if (Session["Name"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(segment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("newSegment", "Admin");
                }
                return View(segment);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult DeleteSegment(int? id)
        {
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Segment segment = db.Segments.Find(id);
                if (segment == null)
                {
                    return HttpNotFound();
                }
                return View(segment);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost, ActionName("DeleteSegment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSegment(int id)
        {
            //Check to see if the user is logged in
            if (Session["Name"] != null)
            {
                Segment segment = db.Segments.Find(id);
                var categories = db.Categories.Where(x => x.Segment_ID == segment.Id);
                List<Assignment> assigment = new List<Assignment>();
                foreach (var item in categories)
                {
                    assigment.AddRange(db.Assignments.Where(x => x.Categories_ID == item.Id));
                }
                List<SubCategory> subCategory = new List<SubCategory>();
                foreach (var item in categories)
                {
                    subCategory.AddRange(db.SubCategories.Where(x => x.Category_ID == item.Id));
                }
                //Remove on cascade
                db.SubCategories.RemoveRange(subCategory);
                db.Assignments.RemoveRange(assigment);
                db.Categories.RemoveRange(categories);
                db.Segments.Remove(segment);
                db.SaveChanges();
                return RedirectToAction("newSegment", "Admin");
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
            
        }

        public ActionResult newCategory()
        {
            if (Session["Name"] != null)
            {
                //Hämta segmentnamn till dopdown i newCategories
                var segmentName = (from s in db.Segments
                                   orderby s.Name
                                   select s).ToList();
                ViewBag.segmentName = segmentName;

                //Join segmentnamn och motsvarande kategorier för lista i newCategories
                var join = (from c in db.Categories
                            join s in db.Segments
                            on c.Segment_ID equals s.Id
                            select new JoinModel { categoryName = c.Name, segmentName = s.Name, categoryID = c.Id, segmentID = s.Id }).ToList();
                ViewBag.join = join;
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult newCategory(Category category)
        {
            if (Session["Name"] != null)
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
                        return RedirectToAction("Error", "Home");
                    }
                }
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.Segment_ID = new SelectList(db.Segments, "ID", "Name", category.Segment_ID);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "ID,Segment_ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("newCategory", "Admin");
            }
            ViewBag.Segment_ID = new SelectList(db.Segments, "ID", "Name", category.Segment_ID);
            return View(category);
        }

        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            var assignments = db.Assignments.Where(x => x.Categories_ID == category.Id);
            var subCategories = db.SubCategories.Where(x => x.Category_ID == category.Id);
            db.SubCategories.RemoveRange(subCategories);
            db.Assignments.RemoveRange(assignments);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("newCategory","Admin");
        }


        public ActionResult NewSubCategory()
        {
            if (Session["Name"] != null)
            {
                //Hämta segmentnamn till dropdown i newCategories
                var categoriesName = (from c in db.Categories
                                      orderby c.Name
                                      select c).ToList();
                ViewBag.categoriesName = categoriesName;

                //Join segmentnamn och motsvarande kategorier för lista i newCategories
                var join = (from s in db.SubCategories
                            join c in db.Categories
                            on s.Category_ID equals c.Id
                            select new JoinModel { subCategoryName = s.Name, subCategoryId = s.Id, categoryName = c.Name, categoryID = c.Id }).ToList();
                ViewBag.join = join;
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult NewSubCategory(SubCategory subCategory)
        {
            if (Session["Name"] != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.SubCategories.Add(subCategory);
                        db.SaveChanges();
                        return RedirectToAction("newSubCategory", "Admin");
                    }
                    //TODO: Add exception, maybe custom error page? 
                    catch (Exception)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                return View();
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult EditSubCategory(int? id)
        {
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubCategory subCategory = db.SubCategories.Find(id);
                if (subCategory == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Category_ID = new SelectList(db.Categories, "Id", "Name", subCategory.Category_ID);
                return View(subCategory);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubCategory([Bind(Include = "Id,Category_ID,URL,Name")] SubCategory subCategory)
        {
            if (Session["Name"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(subCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("NewSubCategory", "Admin");
                }
                ViewBag.Category_ID = new SelectList(db.Categories, "Id", "Name", subCategory.Category_ID);
                return View(subCategory);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult DeleteSubCategory(int? id)
        {
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubCategory subCategory = db.SubCategories.Find(id);
                if (subCategory == null)
                {
                    return HttpNotFound();
                }
                return View(subCategory);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        [HttpPost, ActionName("DeleteSubCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubCategory(int id)
        {
            if (Session["Name"] != null)
            {
                SubCategory subCategory = db.SubCategories.Find(id);
                var assignments = db.Assignments.Where(x => x.SubCategories_ID == subCategory.Id);
                db.Assignments.RemoveRange(assignments);
                db.SubCategories.Remove(subCategory);
                db.SaveChanges();
                return RedirectToAction("NewSubCategory", "Admin");
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult Assignment()
        {
            if (Session["Name"] != null)
            {
                var assignments = db.Assignments.Include(a => a.Category).Include(a => a.SubCategory);
                return View(assignments.ToList());
            }
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public ActionResult newAssignment()
        {
            ViewBag.Categories_ID = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SubCategories_ID = new SelectList(db.SubCategories, "Id", "Name");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult newAssignment(HttpPostedFileBase postedFile, Assignment assignment)
        {
            //Audio file is requierd 
            if (ModelState.IsValid)
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
                assignment.Assignment_Title = "Rubrik";
                assignment.Audio_File = postedFile.FileName;

                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Assignment");
            }

            ViewBag.Categories_ID = new SelectList(db.Categories, "Id", "Name", assignment.Categories_ID);
            ViewBag.SubCategories_ID = new SelectList(db.SubCategories, "Id", "Name", assignment.SubCategories_ID);
            return View(assignment);
        }



        public ActionResult EditAssignments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories_ID = new SelectList(db.Categories, "Id", "Name", assignment.Categories_ID);
            return View(assignment);
        }

        public JsonResult GetSubCategories(int Categories_ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var subCategoriesList = db.SubCategories.Where(x => x.Category_ID == Categories_ID).ToList();
            return Json(subCategoriesList, JsonRequestBehavior.AllowGet);
        }

        // POST: Assignments/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAssignments(HttpPostedFileBase postedFile, Assignment assignment)
        {
            //Audio file is requierd 
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                    assignment.Audio_File = postedFile.FileName;
                }
                assignment.Assignment_Title = "Rubrik";

                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Assignment");
            }

            ViewBag.Categories_ID = new SelectList(db.Categories, "Id", "Name", assignment.Categories_ID);
            ViewBag.SubCategories_ID = new SelectList(db.SubCategories, "Id", "Name", assignment.SubCategories_ID);
            return View(assignment);
        }

        public ActionResult DeleteAssignments(int? id)
        {
            //Check to see if the user is logged in
            if (Session["Name"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Assignment assignment = db.Assignments.Find(id);
                if (assignment == null)
                {
                    return HttpNotFound();
                }
                return View(assignment);
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }


        [HttpPost, ActionName("DeleteAssignments")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAssignments(int id)
        {
            //Check to see if the user is logged in
            if (Session["Name"] != null)
            {
                Assignment assignment = db.Assignments.Find(id);
                db.Assignments.Remove(assignment);
                db.SaveChanges();
                return RedirectToAction("Assignment", "Admin");
            }
            //Else redirect the user back to the login page
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
        }

        public static void GetAssignments ()
        {

        }

        //Dispose db 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}