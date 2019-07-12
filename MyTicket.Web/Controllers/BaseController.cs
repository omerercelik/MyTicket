using MyTicket.Shared.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTicket.Web.Controllers
{
    public class BaseController : Controller
    {
        private const string CURRENT_USER_KEY = "15a20d9b-799e-4087-9005-87642c9cf170";

        // TODO : 
        public AccountModel _currentUser
        {
            get
            {
                return System.Web.HttpContext.Current.Session[CURRENT_USER_KEY] as AccountModel;
            }
            set
            {
                System.Web.HttpContext.Current.Session[CURRENT_USER_KEY] = value;
            }
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName == "Login" ||
                (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Account" && (filterContext.ActionDescriptor.ActionName == "Create" || filterContext.ActionDescriptor.ActionName == "AddUser")) ||
                filterContext.ActionDescriptor.ActionName == "LogOut")
            {
                base.OnActionExecuting(filterContext);

                return;
            }
            if (this._currentUser == null)
            {   // User login fail, return;
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //string loginUrl = Url.Content("~/Account/Login");
                    string loginUrl = "~/Account/Login";
                    if (filterContext.HttpContext.Request != null)
                    {
                        loginUrl += "?ReturnUrl=" + filterContext.HttpContext
                                                                 .Request
                                                                 .Url
                                                                 .AbsoluteUri;
                    }

                    filterContext.Result = Redirect(loginUrl);
                }
                else
                {
                    throw new Exception("Session expired. Please login again to continue");
                }
            }
            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected ActionResult LogOff()
        {
            this._currentUser = null;
            Session.Abandon();

            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            var logoffUrlBuilder =
                new UriBuilder(Request.Url.AbsoluteUri)
                {
                    Path = Url.Action("Login", "Account")
                };
            string url = logoffUrlBuilder.ToString();
            Uri redirectUrl = new Uri(url);
            string address = redirectUrl.AbsoluteUri.Replace("#", "");


            return Json(new { Url = address });
        }
    }
}