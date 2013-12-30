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
  public class AssetController : Controller
  {
    private StockDBEntities db = new StockDBEntities();

    //
    // GET: /Asset/

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];
      return View(db.assets.ToList());
    }

    //
    // GET: /Asset/Details/5

    public ActionResult Details(int id = 0)
    {
      asset asset = db.assets.Find(id);
      if (asset == null)
      {
        return HttpNotFound();
      }
      return View(asset);
    }

    //
    // GET: /Asset/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Asset/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(asset asset)
    {
      if (ModelState.IsValid)
      {
        asset.locked = 0;
        db.assets.Add(asset);
        db.SaveChanges();
        TempData["Success"] = "Asset created succesfully";
        return RedirectToAction("Index");
      }

      return View(asset);
    }

    //
    // GET: /Asset/Edit/5

    public ActionResult Edit(int id = 0)
    {
      asset asset = db.assets.Find(id);
      if (asset == null)
      {
        return HttpNotFound();
      }
      return View(asset);
    }

    //
    // POST: /Asset/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(asset asset)
    {
      if (ModelState.IsValid)
      {
        db.Entry(asset).State = EntityState.Modified;
        db.SaveChanges();
        TempData["Success"] = "Asset edited successfully";
        return RedirectToAction("Index");
      }

      return View(asset);
    }

    //
    // GET: /Asset/Delete/5

    public ActionResult Delete(int id = 0)
    {
      asset asset = db.assets.Find(id);
      if (asset == null)
      {
        return HttpNotFound();
      }
      return View(asset);
    }

    //
    // POST: /Asset/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      asset asset = db.assets.Find(id);
      db.assets.Remove(asset);
      db.SaveChanges();
      TempData["Info"] = "Asset deleted succesfully";
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}