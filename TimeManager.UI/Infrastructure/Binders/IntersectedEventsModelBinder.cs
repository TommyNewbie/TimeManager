using System.Web.Mvc;
using TimeManager.UI.Models.SessionModels;

namespace TimeManager.UI.Infrastructure.Binders
{
    public class IntersectedEventsModelBinder : IModelBinder
    {
        private const string sessionKey = "Intersection";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IntersectedEvents intersection = null;
            intersection = controllerContext.HttpContext.Session[sessionKey] as IntersectedEvents;

            if(intersection == null)
            {
                intersection = new IntersectedEvents { IsEmpty = true };
            }

            return intersection;
        }
    }
}