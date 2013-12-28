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
        return;
      }


      bool logged_in = false;

      if(filterContext.HttpContext.Session["logged_in"] != null){
        logged_in = (bool) filterContext.HttpContext.Session["logged_in"];
      }

      if (!logged_in) {
        string return_to = filterContext.HttpContext.Request.Path.Split('/')[1];
        //filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary{{"action", "Index"},{"controller", "Login"},{"return_to",return_to}});
      }

    }
  }
}