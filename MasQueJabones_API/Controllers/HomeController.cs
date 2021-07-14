using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasQueJabones_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            ViewBag.FrontEnd_URL = ConfigurationManager.AppSettings["FrontEnd_URL"];

            return View();
        }
    }
}
