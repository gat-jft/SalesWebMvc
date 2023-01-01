using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Models;
using System.Diagnostics;

namespace SalesWebMvc_.Net7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Sales Web MVC App from C# Course";
            ViewData["Student"] = "José";

            return View();
            // TENHO VÁRIAS POSSIBILIDADES DE MÉTODOS QUE VÃO ME AUXILIAR A INSTANCIAR O IActionResult (objeto de resposta) COMO UMA RESPOSTA.
            //return View(); // retorna uma (um tipo) ViewResult, ou seja uma View (página).
            //return File(); // retorna um tipo FileResult (arquivo);
            //return Content(); // retorna um ContentResult (conteúdo)
            //return RedirectToAction(); // retorna um RedirectToRouteResult (um Redirecionamento para rota).
            //return Json(); // retorna um JsonResult (Json).
            //return new EmptyResult(); // retorna um EmptyResult (resultado vazio). Este é o melhor modo de não retorna NADA. Peço prá retornar um noo objeto do tipo EmptyResult()
            //return HttpNotFound; // retorna um HttpNotFoundResult.
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}