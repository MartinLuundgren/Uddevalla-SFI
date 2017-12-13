using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Solution.Controllers
{
    public class HomeController : Controller
    {
        //Db connection 
        private SFI_DBEntities db = new SFI_DBEntities();

        public ActionResult Index()
        {
            var getSegments = (from s in db.Segments
                               orderby s.Name
                               select s).ToList();

            return View(getSegments);
        }

        public ActionResult Category(string segment)
        {
            try
            {
                int id = int.Parse(segment);
                var viewCategory = (from c in db.Categories
                                    join s in db.Segments
                                    on c.Segment_ID equals s.ID
                                    where c.Segment_ID == id
                                    select new JoinModelCategory { categories = c, segment = s }).ToList();
                if (viewCategory.Count() > 0)
                {
                    return View(viewCategory);
                }
               return RedirectToAction("Error","Home");
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Home");
            }
        }
        //View all assignments
        public ActionResult Assignment(string category)
        {
            try { 
                int id = int.Parse(category);
                //Gets all assignmets
                var getAssignments = (from a in db.Assignments
                                    where a.Categories_ID == id
                                    select a).ToList();
                if (getAssignments.Count() > 0)
                {
                    foreach (var item in getAssignments)
                    {
                        if (item.Answer_One != null)
                        {
                            item.Answer_One = Regex.Replace(item.Answer_One, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_One = Regex.Replace(item.Answer_One, @"\)", "</span>");
                        }
                        if (item.Answer_Two != null)
                        {
                            item.Answer_Two = Regex.Replace(item.Answer_Two, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_Two = Regex.Replace(item.Answer_Two, @"\)", "</span>");
                        }
                        if (item.Answer_Three != null)
                        {
                            item.Answer_Three = Regex.Replace(item.Answer_Three, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_Three = Regex.Replace(item.Answer_Three, @"\)", "</span>");
                        }
                        if (item.Answer_Four != null)
                        {
                            item.Answer_Four = Regex.Replace(item.Answer_Four, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_Four = Regex.Replace(item.Answer_Four, @"\)", "</span>");
                        }
                        if (item.Answer_Five != null)
                        {
                            item.Answer_Five = Regex.Replace(item.Answer_Five, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_Five = Regex.Replace(item.Answer_Five, @"\)", "</span>");
                        }
                        if (item.Answer_Six != null)
                        {
                            item.Answer_Six = Regex.Replace(item.Answer_Six, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Answer_Six = Regex.Replace(item.Answer_Six, @"\)", "</span>");
                        }
                        if (item.Correct_Answer != null)
                        {
                            item.Correct_Answer = Regex.Replace(item.Correct_Answer, @"\(", "<span style=\"text-decoration: underline; text-decoration-color: red;\">");
                            item.Correct_Answer = Regex.Replace(item.Correct_Answer, @"\)", "</span>");
                        }
                    }
                    return View(getAssignments);
                }
                return RedirectToAction("Error", "Home");
                    
            }
            //Return error
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        //Just return view of error for global
        public ActionResult Error()
        {
            return View();
        }
    }
}