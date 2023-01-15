using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Models;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SellersController : Controller
    {
        public IActionResult Index()
        {          
            return View();
        }
    }
}
