using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace PTIStockMgmt.Controllers
{

  // extension to assembly model to componesate for assembly_asset relation
  public class AssembleRegisterModel : Models.assembly
  {
    // JSON obj key('asset_id') => value('quantity')
    public string assets { get; set; }
  }

  // View of related assembly_assets for each assembly
  public class AssemblyAssetsQuantityModel
  {
    public string title { get; set; }
    public int sap { get; set; }
    public string maker { get; set; }
    public int quantity { get; set; }
  }

  public class AssemblyAndAssetsModel
  {
    public Models.assembly assembly { get; set; }
    public IEnumerable<AssemblyAssetsQuantityModel> assets { get; set; }
  }

  public class AssemblyController : Controller
  {
    private Models.StockDBEntities db = new Models.StockDBEntities();

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];

      // Grab all assemblies together with each assemblies asset list
      var assemblies = (from assembly in db.assemblies
                        select new AssemblyAndAssetsModel
                        {
                          assembly = assembly,
                          assets = (from aa in db.assembly_assets
                                    join asset in db.assets
                                    on aa.asset_id equals asset.id
                                    where aa.assembly_id == assembly.id
                                    select new AssemblyAssetsQuantityModel
                                    {
                                      title = asset.title,
                                      sap = asset.sap,
                                      maker = asset.maker,
                                      quantity = aa.quantity,
                                    })
                        });

      return View(assemblies);
    }

    public ActionResult Create()
    {
      ViewBag.assetinfo = "{}";
      ViewBag.assets = db.assets.ToList();

      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AssembleRegisterModel assembly)
    {
      if (ModelState.IsValid)
      {

        Models.assembly new_assembly = new Models.assembly(assembly);
        db.assemblies.Add(new_assembly);
        db.SaveChanges();

        JObject assets = JObject.Parse(assembly.assets);
        foreach (var asset in assets)
        {
          db.assembly_assets.Add(new Models.assembly_assets(new_assembly.id, int.Parse(asset.Key), (int) asset.Value, ""));
        }
        db.SaveChanges();

        return RedirectToAction("Index");
      }

      ViewBag.assetinfo = assembly.assets;
      ViewBag.assets = db.assets.ToList();

      return View(assembly);
    }

    //
    // GET: /Assembly/Edit/5

    public ActionResult Edit(int id = 0)
    {
      Models.assembly assembly = db.assemblies.Find(id);

      if (assembly == null)
      {
        return HttpNotFound();
      }

      var assets = db.assembly_assets.Where(aa => aa.assembly_id == assembly.id);

      JObject quantity_obj = new JObject();

      foreach (var asset in assets)
      {
        quantity_obj[asset.asset_id.ToString()] = asset.quantity;
      }

      ViewBag.assetinfo = quantity_obj;
      ViewBag.assets = db.assets.ToList();

      return View(assembly);
    }

    //
    // POST: /Assembly/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(AssembleRegisterModel assembly)
    {
      if (ModelState.IsValid)
      {

        Models.assembly new_assembly = new Models.assembly(assembly);
        db.Entry(new_assembly).State = EntityState.Modified;

        foreach (var row in db.assembly_assets.Where(aa => aa.assembly_id == assembly.id))
        {
          db.assembly_assets.Remove(row);
        }

        JObject components = JObject.Parse(assembly.assets);

        foreach (var prop in components)
        {
          db.assembly_assets.Add(new Models.assembly_assets ( assembly.id, int.Parse(prop.Key), (int)prop.Value,  "" ));

        }
        db.SaveChanges();

        return RedirectToAction("Index");
      }

      ViewBag.assetinfo = assembly.assets;
      ViewBag.assets = db.assets.ToList();

      return View(assembly);
    }

    //
    // GET: /Assembly/Delete/5

    public ActionResult Delete(int id = 0)
    {
      Models.assembly assembly = db.assemblies.Find(id);
      if (assembly == null)
      {
        return HttpNotFound();
      }
      return View(assembly);
    }

    //
    // POST: /Assembly/Delete/5

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      Models.assembly assembly = db.assemblies.Find(id);
      db.assemblies.Remove(assembly);
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