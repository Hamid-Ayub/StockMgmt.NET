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

  public class AssembleRegisterModel : Models.assembly
  {
    public string assets { get; set; }
  }

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

  public class AssembleViewModel
  {
    public List<Models.asset> assets { get; set; }
    public Models.assembly assembly { get; set; }
  }

  public class AssemblyController : Controller
  {
    private Models.StockDBEntities db = new Models.StockDBEntities();

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];

      var assemblies = (from a in db.assemblies
                        select new AssemblyAndAssetsModel
                        {
                          assembly = a
                        }).ToList();

      for (int i = 0; i < assemblies.Count(); i++)
      {
        int id = assemblies[i].assembly.id;
        var list = (from aa in db.assembly_assets
                    join ass in db.assets
                    on aa.asset_id equals ass.id
                    where aa.assembly_id == id
                    select new AssemblyAssetsQuantityModel
                    {
                      title = ass.title,
                      sap = ass.sap,
                      maker = ass.maker,
                      quantity = aa.quantity,
                    }
                    ).ToList();
        assemblies[i].assets = list;
      }

      return View(assemblies);
    }

    public ActionResult Create()
    {
      return View(new AssembleViewModel { assets = (from a in db.assets select a).ToList() });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(AssembleRegisterModel assembly)
    {
      if (ModelState.IsValid)
      {

        Models.assembly new_assembly = new Models.assembly
        {
          name = assembly.name,
          comment = assembly.comment,
          tags = assembly.tags,
          parent = assembly.parent,
          weight = assembly.weight,
          volume = assembly.volume,
          retail = assembly.retail,
          wholesale = assembly.wholesale
        };

        db.assemblies.Add(new_assembly);
        db.SaveChanges();

        JObject components = JObject.Parse(assembly.assets);

        foreach (var prop in components)
        {
          db.assembly_assets.Add(new Models.assembly_assets
          {
            assembly_id = new_assembly.id,
            asset_id = int.Parse(prop.Key),
            quantity = (int)prop.Value,
            comment = ""
          });

        }
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(new AssembleViewModel { assets = (from a in db.assets select a).ToList() });
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

      var assets = (from a in db.assembly_assets where a.assembly_id == assembly.id select a).ToList();

      JObject obj = new JObject();

      for (int i = 0; i < assets.Count; i++)
      {
        obj[assets[i].asset_id.ToString()] = assets[i].quantity;
      }

      ViewBag.assetinfo = obj;

      return View(new AssembleViewModel { assets = (from a in db.assets select a).ToList(), assembly = assembly });
    }

    //
    // POST: /Assembly/Edit/5

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(AssembleRegisterModel assembly)
    {
      if (ModelState.IsValid)
      {

        Models.assembly new_assembly = new Models.assembly
        {
          id = assembly.id,
          name = assembly.name,
          comment = assembly.comment,
          tags = assembly.tags,
          parent = assembly.parent,
          weight = assembly.weight,
          volume = assembly.volume,
          retail = assembly.retail,
          wholesale = assembly.wholesale
        };

        db.Entry(new_assembly).State = EntityState.Modified;


        var delete_query = from aa in db.assembly_assets where aa.assembly_id == assembly.id select aa;

        foreach (var row in delete_query)
        {
          db.assembly_assets.Remove(row);
        }

        db.SaveChanges();

        JObject components = JObject.Parse(assembly.assets);

        foreach (var prop in components)
        {
          db.assembly_assets.Add(new Models.assembly_assets
          {
            assembly_id = assembly.id,
            asset_id = int.Parse(prop.Key),
            quantity = (int)prop.Value,
            comment = ""
          });

        }
        db.SaveChanges();

        return RedirectToAction("Index");
      }

      ViewBag.assetinfo = assembly.assets;

      return View(new AssembleViewModel { assets = (from a in db.assets select a).ToList(), assembly = assembly });
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