
using Microsoft.AspNetCore.Mvc;

namespace Menu.Controllers
{
    public class MenuController : Controller
    {
        // GET: MenuController
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult MenuApp()
        {
            return View("Index");
        }


        public ActionResult WardsCardsApp()
        {
            return View("Index");
        }


    }
}
