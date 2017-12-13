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
        private SFI_DBEntities db = new SFI_DBEntities();
        
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Name"] != null)
            {
                return View();
            }
            //Else redirect the user back to the login page
            //test
            else
            {
                return RedirectToAction("/Index", "Login", new { area = "" });
            }
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
                    segment.URL= Regex.Replace(segment.URL, @"watch\W\w\W", "embed/");
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

        public ActionResult EditSegment(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSegment([Bind(Include = "ID,URL,Name")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                segment.URL = Regex.Replace(segment.URL, @"watch\W\w\W", "embed/");
                db.Entry(segment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("newSegment", "Admin");
            }
            return View(segment);
        }

        public ActionResult DeleteSegment(int? id)
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

        [HttpPost, ActionName("DeleteSegment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSegment(int id)
        {
            Segment segment = db.Segments.Find(id);
            var categories = db.Categories.Where(x => x.Segment_ID == segment.ID);
            List<Assignment> assigment = new List<Assignment>();
            foreach (var item in categories)
            {
                assigment.AddRange(db.Assignments.Where(x => x.Categories_ID == item.ID));
            }
            db.Assignments.RemoveRange(assigment);
            db.Categories.RemoveRange(categories);
            db.Segments.Remove(segment);
            db.SaveChanges();
            return RedirectToAction("newSegment","Admin");
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
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("newCategory","Admin");
        }


        [HttpGet]
        public ActionResult newAssignment()
        {
            var assignments = (from s in db.Assignments
                                select s).ToList();
            ViewBag.assignments = assignments;

            var categoryName = (from s in db.Categories
                               orderby s.Name
                               select s).ToList();
            ViewBag.categoryName = categoryName;

            var joinAssignmentsOnCategories = (from a in db.Assignments
                        join c in db.Categories
                        on a.Categories_ID equals c.ID
                        select new JoinModelAssignments { categoryName = c.Name, categoryID = c.ID, assignmentID = a.ID, assignmentAudio = a.Audio_File, assignmentType = a.Assignment_Type, assignmentCorrectAnswer = a.Correct_Answer, assignmentAnswerOne = a.Answer_One, assignmentAnswerTwo = a.Answer_Two, assignmentAnswerThree = a.Answer_Three, assignmentAnswerFour = a.Answer_Four, assignmentAnswerFive = a.Answer_Five, assignmentAnswerSix = a.Answer_Six }).ToList();
            ViewBag.joinAssignmentsOnCategories = joinAssignmentsOnCategories;

            return View();
        }

        [HttpPost]
        public ActionResult newAssignment(HttpPostedFileBase postedFile, Assignment assignment)
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
            assignment.Audio_File = postedFile.FileName;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Assignments.Add(assignment);
                    db.SaveChanges();
                    return RedirectToAction("newAssignment", "Admin");
                }
                //TODO: Add exception, maybe custom error page? 
                catch (Exception)
                {
                    throw;
                }
            }
            return View();
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
            ViewBag.Categories_ID = new SelectList(db.Categories, "ID", "Name", assignment.Categories_ID);
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAssignments([Bind(Include = "ID,Categories_ID,Assignment_Type,Audio_File,Answer_One,Answer_Two,Answer_Three,Answer_Four,Answer_Five,Answer_Six,Correct_Answer")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("newAssignment", "Admin");
            }
            ViewBag.Categories_ID = new SelectList(db.Categories, "ID", "Name", assignment.Categories_ID);
            return View(assignment);
        }
        
        public ActionResult DeleteAssignments(int? id)
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


        [HttpPost, ActionName("DeleteAssignments")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAssignments(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("newAssignment", "Admin");
        }

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