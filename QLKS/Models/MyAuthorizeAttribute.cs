using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLKS.Models
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //var isAuthorized= base.OnAuthorization(filterContext);
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                using (var db = new QLKSEntities2())
                {
                    var authorizedRoles = (from u in db.TaiKhoan
                                           where u.Email == filterContext.HttpContext.User.Identity.Name
                                           select u.Roles).SingleOrDefault();
                    Roles = string.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;
                }
            }
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.TempData.Add("RedirectReason", "Unlogin");
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
        }
    }
}