using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        base.OnActionExecuting(filterContext);

        if (HttpContext.Session.GetString("email") == null)
        {
            filterContext.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
