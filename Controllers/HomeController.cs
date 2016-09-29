using PluralsightDownloader.Web.ViewModel;
using System;
using System.Web.Mvc;

namespace PluralsightDownloader.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(HomeModel model)
        {
            ViewBag.Title = "Home Page";

            if (String.IsNullOrEmpty(model.course))
            {
                model.course = "node-application-patterns";
            }

            return View(model);
        }
    }
}