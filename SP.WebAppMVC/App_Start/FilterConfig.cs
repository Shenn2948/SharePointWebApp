using System.Web.Mvc;
using SP.WebAppMVC.CustomFilters;

namespace SP.WebAppMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleError());
        }
    }
}
