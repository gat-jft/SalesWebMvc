using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Models;

namespace SalesWebMvc_.Net7.Controllers
{
    public class DepartmentsController : Controller
    {
        public IActionResult Index()
        {
            List<Department> list = new List<Department>();
            list.Add(new Department { Id = 1, Name = "Electronics"});
            list.Add(new Department { Id = 2, Name = "Fashion" });  
            //list.Add(new Department { Id = 3, Name = "Tools" });          

            return View(list); // Enviando a minha lista com os dados para minha View (Tela).       Criar uma subpasta com o mesmo nome do Controlador (prefixo dele) na pasta Views.        Criar a View (arquivo .cshtml que será nossa Tela), usando o gerador automático na ferramenta automática: Botão direito na subpasta Departments / Add / View / Exibição do Razor.
        }
    }
}
