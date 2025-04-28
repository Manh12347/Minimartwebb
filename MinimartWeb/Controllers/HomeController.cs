using Microsoft.AspNetCore.Mvc;

namespace MinimartWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // This should render the Index view
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
