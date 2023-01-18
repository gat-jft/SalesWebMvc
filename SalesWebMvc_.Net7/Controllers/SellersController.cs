using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Models;
using SalesWebMvc_.Net7.Services;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SellersController : Controller
    {        
        // Pra que o Index() chame o FindAll() lá do SellerService, vamos ter que declarar uma dependência (Property, não injeção de dependência que é outra coisa) com o SellerService.
        private readonly SellerService _sellerService;

        // Vamos fazer o nosso Construtor pra ele injetar a dependência.
        public  SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        // Este método Index() vai ter que chamar a operação FindAll(), lá do SellerService.
        // Antes  disso, temos que declarar uma dependência para o SellerService (que é o _sellerService). Depois, devemos fazer o Construtor (public SellerSevice) deste Controller, prá ele INJETAR A DEPENDÊNCIA do serviço (SellerService).
        public IActionResult Index()
        {
            // Poderia ser declarado no lugar do Tipo var (genérico), o Tipo List<Seller>.
            // A operação FindAll() do nosso serviço SellerService, vai me retornar uma Lista de Seller.
            var list = _sellerService.FindAll().ToList();

            // Agora, vou passar esta lista, como argumento no meu método View(), prá que ele gerar um IActionResult contendo esta lista (list).
            return View(list);
        }

        // A dinâmica do MVC acontecendo neste exemplo:
        // - Eu (usuário no navegador) chamei o Controlador (http.../Seller/Index)
        // - O Controlador acessou o meu Model: executou no Index() o comando: var list = _sellerService.FindAll().ToList();
        // - O Controlador pegou o dado: a lista retornada do comando anterior.
        // - E o Controlador ele vai encaminhar esses dados (list) para a minha View: return View(list);.
        //
        // Note que é o controlador que é o meio-de-campo entre Model e a View.
    }
}
