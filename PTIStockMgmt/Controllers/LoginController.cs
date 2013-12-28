using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PTIStockMgmt.Models;

namespace PTIStockMgmt.Controllers
{
    public class LoginController : Controller
    {
        private StockDBEntities db = new StockDBEntities();

        public ActionResult Index(string return_to = "")
        {

          if (return_to.Count() > 0)
          {
            ViewBag.return_to = return_to;
          }

          return View();
        }

        // POST: /Login
        [HttpPost]
        public ActionResult Index(FormCollection value)
        {

          string username = value["username"].ToString();
          user usr = db.users.SingleOrDefault(u => u.username == username);

          if (usr == null)
          {
            ViewBag.Danger = "Incorrect Username or Password.";
            return View(usr);
          }

          if (usr.password != value["password"])
          {
            ViewBag.Danger = "Incorrect Username or Password.";
            return View(usr);
          }

          Session["logged_in"] = true;

          TempData["Success"] = "Logged in successfully.";

          if (value["return_to"] != null && value["return_to"].Count() > 0)
          {
            return RedirectToAction("Index", value["return_to"]);
          }
          else
          {
            return RedirectToAction("Index", "Asset");
          }

        }

        // GET: /Login/Logout
        public ActionResult Logout()
        {
          Session["logged_in"] = false;
          ViewBag.Success = "Logged out successfully.";
          return View("Index");
        }

    }
}
