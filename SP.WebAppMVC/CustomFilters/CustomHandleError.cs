using System;
using System.Net;
using System.Web.Mvc;

namespace SP.WebAppMVC.CustomFilters
{
    public class CustomHandleError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is Exception ex)
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}