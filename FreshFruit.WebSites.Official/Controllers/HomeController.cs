using FreshFruit.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreshFruit.WebSites.Official
{
    public class HomeController : Controller
    {
        private IProductRepository repository;
        public HomeController(IProductRepository irepository)
        {
            repository = irepository;
        }

        public ActionResult Index()
        {
            repository.AddObject(2);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}