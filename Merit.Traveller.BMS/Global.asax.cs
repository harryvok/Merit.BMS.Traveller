using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Merit.Traveller.BMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new String[] { "Merit.Traveller.BMS.Controllers" }
            );



        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            //Creating bundle for your css files
            bundles.Add(new StyleBundle("~/Content/css/BMS").Include("~/Content/bootstrap/bootstrap.min.css",
            "~/Content/CSS/Base.css",
            "~/Content/CSS/Layout.css",
            "~/Content/CSS/Module.css",
            "~/Content/CSS/State.css",
            "~/Content/CSS/jquery-ui.css",
            "~/Content/CSS/ace-min.css",
            "~/Scripts/jtable/themes/metro/blue/jtable.css"));

            //Creating bundle for your js files
            bundles.Add(new ScriptBundle("~/Content/scripts/BMS").Include(
            "~/Scripts/Libraries/jquery-1.9.1.min.js",
            "~/Scripts/Libraries/bootstrap.min.js",
            "~/Scripts/Libraries/jquery-ui-1.10.3.min.js",
            "~/Scripts/Libraries/modernizr-2.5.3.js",
            "~/Scripts/Libraries/knockout-2.3.0.js",
            "~/Scripts/Libraries/jquery.validate.js",
            "~/Scripts/Libraries/jquery-timepicker.js",
            "~/Scripts/Libraries/jquery.form.min.js",
            "~/Scripts/jquery.dataTables.js",
            "~/Scripts/sorttable.js",
            "~/Scripts/jtable/jquery.jtable.js"));
        }

        protected void Application_Start()
        {
            RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true; 
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}