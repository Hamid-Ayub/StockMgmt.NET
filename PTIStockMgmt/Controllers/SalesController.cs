﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTIStockMgmt.Controllers
{
  public class SalesController : Controller
  {
    //
    // GET: /Sales/

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];

      ViewBag.Danger = "Order PO 10231 unpaid and overdue";
      return View();
    }

  }
}
