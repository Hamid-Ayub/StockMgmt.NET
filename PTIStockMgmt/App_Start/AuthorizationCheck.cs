using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace PTIStockMgmt
{
  public class AuthorizationCheck : FilterAttribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationContext filterContext)
    {

      if (filterContext.HttpContext.Request.Path.Contains("Login"))
      {
        // Skip authorizaton check if at login screen
        return;
      }

      var logged_in = filterContext.HttpContext.Session["logged_in"];

      if (logged_in != null && (bool)logged_in == true)
      {
        // Logged in, allow user through
        return;
      }

      string return_to = filterContext.HttpContext.Request.Path.Split('/')[1];
      filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary{{"action", "Index"},{"controller", "Login"},{"return_to",return_to}});

    }
  }
}