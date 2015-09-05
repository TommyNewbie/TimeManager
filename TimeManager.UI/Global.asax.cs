using System.Web.Mvc;
using System.Web.Routing;
using TimeManager.UI.Infrastructure.Binders;
using TimeManager.UI.Models.SessionModels;

namespace TimeManager.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(IntersectedEvents), new IntersectedEventsModelBinder());
        }
    }
}
