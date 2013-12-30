using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt.Models;
using System.Security.Cryptography;

namespace PTIStockMgmt.Controllers
{
  public class UserController : Controller
  {
    private StockDBEntities db = new StockDBEntities();

    //
    // GET: /User/

    public ActionResult Index()
    {

      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];
      return View(db.users.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /User/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(FormCollection value)
    {

      user usr = new user();

      usr.joined = DateTime.Now;
      usr.name = value["name"];
      usr.email = value["email"];
      usr.salt = Crypto.GenSalt();
      usr.password = Crypto.Hash(usr.salt, value["password"]);
      db.users.Add(usr);
      db.SaveChanges();
      return RedirectToAction("Index");

    }

    //
    // GET: /User/Edit/5

    public ActionResult Edit(int id = 0)
    {
      user user = db.users.Find(id);
      if (user == null)
      {
        return HttpNotFound();
      }
      return View(user);
    }

    //
    // POST: /User/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(FormCollection value)
    {
      int id = int.Parse(value["id"]);
      user usr = db.users.SingleOrDefault(u => u.id == id);

      if (ModelState.IsValid)
      {
        usr.email = value["email"];
        usr.name = value["name"];
        usr.password = Crypto.Hash(usr.salt, value["password"]);
        db.Entry(usr).State = EntityState.Modified;
        db.SaveChanges();
        TempData["Success"] = "User edited successfully";
        return RedirectToAction("Index");
      }
      return View(usr);
    }

    //
    // GET: /User/Delete/5

    public ActionResult Delete(int id = 0)
    {
      user user = db.users.Find(id);
      if (user == null)
      {
        return HttpNotFound();
      }
      return View(user);
    }

    //
    // POST: /User/Delete/5

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      user user = db.users.Find(id);
      db.users.Remove(user);
      db.SaveChanges();
      TempData["Info"] = "User deleted successfully";
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }


  }
}