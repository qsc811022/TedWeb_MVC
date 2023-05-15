using Microsoft.AspNetCore.Mvc;

namespace TedWeb.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
