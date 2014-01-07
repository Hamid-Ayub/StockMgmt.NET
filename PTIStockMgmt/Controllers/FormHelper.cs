using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PTIStockMgmt.Helpers
{
  public static class FormExtensions
  {

    public static IHtmlString Submit(this HtmlHelper helper, string name = "Create")
    {
      return new HtmlString(String.Format("<a href='#' onclick='$(\"form\").submit()' class='btn btn-success'>{0}</a>", "Create"));
    }

    public static IHtmlString Cancel(this HtmlHelper helper)
    {
      var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
      var url = urlHelper.Action("Index");
      return new HtmlString(String.Format("<a href='{0}' class='btn'>Cancel</a>", url));
    }

    public static IHtmlString Btn(this HtmlHelper helper, string btnClass, string value, string controller, string action)
    {
      var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
      var url = urlHelper.Action(action, controller);
      return new HtmlString(String.Format("<a href='{0}' class='btn {1}'>{2}</a>", url, btnClass, value));
    }

    public static IHtmlString BtnSuccess(this HtmlHelper helper, string value, string controller, string action = "Index")
    {
      return Btn(helper, "btn-success", value, controller, action);
    }

    public static IHtmlString FormGroup<TModel, TProperty>(
        this HtmlHelper<TModel> helper, 
        Expression<Func<TModel, TProperty>> ex,
        string title,
        string classAttr = ""
    )
    {
      var pre = string.Format("<div class='control-group'><label class='control-label'>{0}</label><div class='controls'>",title);
      var textbox = helper.TextBoxFor(ex, null, new { placeholder = title, @class = classAttr, type = "text" });
      var validation = helper.ValidationMessageFor(ex);
      var post = "</div></div>";

      return new HtmlString(pre + textbox + validation + post);
    }

  }
}