using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt.Models;

namespace PTIStockMgmt.Controllers
{
    public class LocationController : Controller
    {
        private StockDBEntities db = new StockDBEntities();

        //
        // GET: /Location/

        public ActionResult Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Danger = TempData["Danger"];
            ViewBag.Info = TempData["Info"];
            return View(db.locations.ToList());
        }

        //
        // GET: /Location/Details/5

        public ActionResult Details(int id = 0)
        {
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // GET: /Location/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Location/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(location location)
        {
            if (ModelState.IsValid)
            {
                db.locations.Add(location);
                db.SaveChanges();
                TempData["Success"] = "Location created successfully";
                return RedirectToAction("Index");
            }

            return View(location);
        }

        //
        // GET: /Location/Edit/5

        public ActionResult Edit(int id = 0)
        {
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Location/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "Location edited successfully";
                return RedirectToAction("Index");
            }
            return View(location);
        }

        //
        // GET: /Location/Delete/5

        public ActionResult Delete(int id = 0)
        {
            location location = db.locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        //
        // POST: /Location/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            location location = db.locations.Find(id);
            db.locations.Remove(location);
            db.SaveChanges();
            TempData["Info"] = "Location deleted successfully";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}