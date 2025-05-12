using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
