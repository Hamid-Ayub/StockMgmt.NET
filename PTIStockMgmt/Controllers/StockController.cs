using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt;
using Newtonsoft.Json.Linq;

namespace PTIStockMgmt.Controllers
{

  public class ActiveAssetViewModel
  {
    public int id { get; set; }
    public int asset_id { get; set; }
    public int location_id { get; set; }
    public int quantity { get; set; }
    public string location_string { get; set; }
    public string title { get; set; }

  }

  public class StockController : Controller
  {
    private Models.StockDBEntities db = new Models.StockDBEntities();

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];

      var stock = (from active in db.active_assets
                   join location in db.locations
                   on active.location_id equals location.id
                   join type in db.assets
                   on active.asset_id equals type.id
                   select new ActiveAssetViewModel
                   {
                     id = active.id,
                     quantity = active.quantity,
                     location_id = location.id,
                     location_string = location.location_string,
                     asset_id = type.id,
                     title = type.title
                   });

      return View(stock);
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
    public ActionResult Create(Models.active_assets active_assets)
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
      Models.active_assets active_assets = db.active_assets.Find(id);

      if (active_assets == null)
      {
        return HttpNotFound();
      }

      JObject location2id = new JObject();
      foreach (var location in db.locations.ToList())
      {
        location2id[location.location_string] = location.id;
      }

      JObject asset2id = new JObject();
      foreach(var assetd in db.assets.ToList()){
        asset2id[assetd.title] = assetd.id;
      }

      ViewBag.location2id = location2id;
      ViewBag.asset2id = asset2id;
      ViewBag.location = db.locations.Where(location => location.id == active_assets.location_id).FirstOrDefault().location_string;
      ViewBag.asset = db.assets.Where(asset => asset.id == active_assets.id).FirstOrDefault().title;

      return View(active_assets);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(PTIStockMgmt.Models.active_assets active_assets)
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
      PTIStockMgmt.Models.active_assets active_assets = db.active_assets.Find(id);
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
      Models.active_assets active_assets = db.active_assets.Find(id);
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