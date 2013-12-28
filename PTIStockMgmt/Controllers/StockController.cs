using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt.Models;
using Newtonsoft.Json.Linq;

namespace PTIStockMgmt.Controllers
{
    public class StockController : Controller
    {
        private StockDBEntities db = new StockDBEntities();

        public ActionResult Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Danger = TempData["Danger"];
            ViewBag.Info = TempData["Info"];

            ViewBag.AssetTitle = "OpticalFiber";
            ViewBag.LocationString = "1 Birdwodd";
            return View(db.active_assets.ToList());
        }

        public ActionResult Create()
        {

         var locations = (from dl in db.locations select dl).ToList();
         var assets = (from da in db.assets select da).ToList();

         JObject location2id = new JObject();

         for (int i = 0; i < locations.Count(); i++)
         {
           location2id[locations[i].location_string] = locations[i].id;
         }

         JObject asset2id = new JObject();
         for (int i = 0; i < locations.Count(); i++)
         {
           asset2id[assets[i].title] = assets[i].id;
         }

        ViewBag.location2id = location2id;
        ViewBag.asset2id = asset2id;

          return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(active_assets active_assets)
        {
            if (ModelState.IsValid)
            {
                db.active_assets.Add(active_assets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(active_assets);
        }

        public ActionResult Edit(int id = 0)
        {
            active_assets active_assets = db.active_assets.Find(id);
            if (active_assets == null)
            {
                return HttpNotFound();
            }

         var locations = (from dl in db.locations select dl).ToList();
         var assets = (from da in db.assets select da).ToList();

         JObject location2id = new JObject();

         for (int i = 0; i < locations.Count(); i++)
         {
           location2id[locations[i].location_string] = locations[i].id;
         }

         JObject asset2id = new JObject();
         for (int i = 0; i < locations.Count(); i++)
         {
           asset2id[assets[i].title] = assets[i].id;
         }

        ViewBag.location2id = location2id;
        ViewBag.asset2id = asset2id;
        ViewBag.location = (from lo in db.locations where lo.id == active_assets.location_id select lo.location_string).FirstOrDefault().ToString();
        ViewBag.asset = (from ass in db.assets where ass.id == active_assets.asset_id select ass.title).FirstOrDefault().ToString();

            return View(active_assets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(active_assets active_assets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(active_assets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(active_assets);
        }

        public ActionResult Delete(int id = 0)
        {
            active_assets active_assets = db.active_assets.Find(id);
            if (active_assets == null)
            {
                return HttpNotFound();
            }
            return View(active_assets);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            active_assets active_assets = db.active_assets.Find(id);
            db.active_assets.Remove(active_assets);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}