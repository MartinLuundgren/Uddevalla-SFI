﻿using System;
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
            var Segments = (from s in db.Segments select s).ToList();
            //Test
            //En kommentar för att synka
            return View(Segments);
        }
        [HttpPost]
        public ActionResult Index(Segment segment)
        {
            db.Segments.Add(segment);
            db.SaveChanges();
            return View();
        }

        public ActionResult newSegment()
        {
            //Saknar vy
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