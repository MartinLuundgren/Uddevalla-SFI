using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Solution.Models;

namespace Solution.Controllers
{
    public class StartPageMoviesController : Controller
    {
        private SfiDbEntities db = new SfiDbEntities();

        // GET: StartPageMovies
        public ActionResult Index()
        {
            return View(db.StartPageMovies.ToList());
        }

        // GET: StartPageMovies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StartPageMovie startPageMovie = db.StartPageMovies.Find(id);
            if (startPageMovie == null)
            {
                return HttpNotFound();
            }
            return View(startPageMovie);
        }

        // GET: StartPageMovies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StartPageMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,URL")] StartPageMovie startPageMovie)
        {
            if (ModelState.IsValid)
            {
                db.StartPageMovies.Add(startPageMovie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(startPageMovie);
        }

        // GET: StartPageMovies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StartPageMovie startPageMovie = db.StartPageMovies.Find(id);
            if (startPageMovie == null)
            {
                return HttpNotFound();
            }
            return View(startPageMovie);
        }

        // POST: StartPageMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,URL")] StartPageMovie startPageMovie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(startPageMovie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(startPageMovie);
        }

        // GET: StartPageMovies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StartPageMovie startPageMovie = db.StartPageMovies.Find(id);
            if (startPageMovie == null)
            {
                return HttpNotFound();
            }
            return View(startPageMovie);
        }

        // POST: StartPageMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StartPageMovie startPageMovie = db.StartPageMovies.Find(id);
            db.StartPageMovies.Remove(startPageMovie);
            db.SaveChanges();
            return RedirectToAction("Index");
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
