﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PTIStockMgmt.Controllers
{
  public class ClientsController : Controller
  {
    //
    // GET: /Clients/

    public ActionResult Index()
    {
      ViewBag.Success = TempData["Success"];
      ViewBag.Danger = TempData["Danger"];
      ViewBag.Info = TempData["Info"];

      return View();
    }

  }
}
