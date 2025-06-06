﻿
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Menu.Controllers
{
    public class MenuController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public MenuController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: MenuController
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult MenuApp()
        {
            return View("Index");
        }


        public ActionResult WordsCardsApp()
        {
            return View("Index");
        }

        public ActionResult TaskManagementApp()
        {
            return View("Index");
        }
        
        public ActionResult PlaningPoker()
        {
            return View("Index");

            //return File(_env.ContentRootPath + "\\wwwroot\\index.html", "text/html");
            //return File("\\index.html", "text/html");
            //return View("Index");
        }

        public ActionResult VaultApp()
        {
            return View("Index");

        }

    }
}
