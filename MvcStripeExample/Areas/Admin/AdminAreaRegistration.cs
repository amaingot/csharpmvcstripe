using System.Web.Mvc;

namespace MvcStripeExample.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    name: "Admin_subdomain",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new {controller = "Dashboard", action = "index", id = UrlParameter.Optional},
            //    constraints: new {subdomain = "admin"}
            //);

            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}