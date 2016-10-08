using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApp.Safari
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Setting settings;
        public static bool inited;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            if (!inited)
            {
                inited = true;
                MvcApplication.settings = Setting.Load(SettingClientType.Web);
            }
        }


    }
}
