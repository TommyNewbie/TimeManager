using System.Web.Mvc;
using System.Web.Routing;

namespace TimeManager.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("default", "{controller}/{action}", new { controller = "Events", action = "Events" });
        }
    }
}
