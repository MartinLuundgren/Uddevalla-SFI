using Solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solution.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private SFI_DBEntities db = new SFI_DBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        /*public ActionResult Uppladdning()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Uppladdning(Segment nyttSegment)
        {
            db.Segments.Add(nyttSegment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Radering()
        {
            List<Texter> textLista = db.Texter.ToList();
            return View(textLista);
        }
        public ActionResult Radera(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Radering");
            }
            Texter raderadText = db.Texter.Find(id);
            return View(raderadText);
        }
        [HttpPost]
        public ActionResult Radera(int id)
        {
            Texter raderadText = db.Texter.Find(id);
            db.Texter.Remove(raderadText);
            db.SaveChanges();
            return RedirectToAction("Radering");
        }
        public ActionResult Uppdatering()
        {
            List<Texter> textLista = db.Texter.ToList();
            return View(textLista);
        }
        public ActionResult Uppdatera(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Uppdatering");
            }
            Texter uppdateradText = db.Texter.Find(id);
            return View(uppdateradText);
        }
        [HttpPost]
        public ActionResult Uppdatera(Texter uppdateradText)
        {
            db.Entry(uppdateradText).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Uppdatering");
        }
        public ActionResult Utloggning()
        {
            return RedirectToAction("../Home/Index");
        }*/
    }
}