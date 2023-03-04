using Microsoft.AspNetCore.Mvc;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SalesRecordsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // SimpleSearch = Busca simples.
        public IActionResult SimpleSearch()
        {
            return View();
        }

        // SimpleSearch = Busca simples.
        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}
