using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Fresh Fruit";

            return View();
        }

        public ActionResult IndexEntrance() 
        {
            return View();
        }

        public ActionResult WechatEntrance()
        {
            return View();
        }

        public ActionResult AdminEntrance()
        {
            return View();
        }

    }
}
