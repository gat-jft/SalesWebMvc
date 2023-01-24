using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Models; // Para enxergar o Seller.
using SalesWebMvc_.Net7.Services;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SellersController : Controller
    {
        // Pra que o Index() chame o FindAll() lá do SellerService, vamos ter que declarar uma dependência (Property, não injeção de dependência que é outra coisa) com o SellerService.
        private readonly SellerService _sellerService;       
         

        // Vamos fazer o nosso Construtor pra ele injetar a dependência.
        public SellersController(SellerService sellerService)
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


        //Ação para criar um Novo Vendedor.
        public IActionResult Create()
        {
            return View();
        }

        // Prá que eu receba este objeto da requisição, e instancie este Vendedor, basta eu colocar ele (este objeto) aqui como parâmetro do Método.
        [HttpPost]                       // Com esta ANNOTATION [HttpPost], eu estou indicando que esta minha Ação (Método                       // ANNOTATION [HttpPost].   ANNOTATION é uma Classe (tipo). Esta ANNOTATION [HttpPost], ele Identifica uma ação (este método Create) é uma Ação de POST e não de GET.         // , que será uma View / Página) que suporta o método HTTP POST.     POST é quando eu tenho uma Operação (Ação) que altera algum dado, como por exemplo, qualquer alteração no BD, seja ela inclusão / deleção etc.      // Então, ela só pode ser usada em Método. NÃO POSSO COLOCAR NUMA class.     // Para uma Ação que usa o método POST.       // Para toda ação POST que eu criar, tenho que ter uma Ação GET, senão não funciona, não renderiza a página.   Por isso que tive que ter o Método anterior, com o mesmo Nome, e SEM o POST, indicando que é GET.    MAS sem parâmetro.     deste Método Então, este  A Ação anterior de mesmo nome é obrigatória, senão não gera a página.
        [ValidateAntiForgeryToken]       // Validar token antifalsificação.     // ANNOTATION [ValidateAntiForgeryToken] é prá prevenir que a minha aplicação sofra ataque CSRF.        // Este tipo de ataque é quando alguém aproveita a minha SESSÃO de autenticação, e envia dados maliciosos aproveitando a minha autenticação.       // Para mais detalhes sobre isso, ver o link do material de apoio.       // Esta ANNOTATION [ValidateAntiForgeryToken], ela especifica que a classe ou método ao qual este atributo é aplicado valida o token anti-falsificação. Se o token antifalsificação não estiver disponível ou se o token for inválido, a validação falhará e o método de ação não será executado.      // Observações:    Este atributo ajuda na defesa contra a falsificação de solicitação entre sites. Isso não impedirá outros ataques de falsificação ou adulteração.           // Token é: Token é um dispositivo eletrônico gerador de senhas, geralmente sem conexão física com o computador, podendo também, em algumas versões, ser conectado a uma porta USB (porta de pendrives). Conectado a uma porta USB, no caso é um pendrive com um programa gerador de senhas (programa token).
        public IActionResult Create(Seller seller)
        {
            // Inseriu + este Vendedor no BD.
            _sellerService.Insert(seller);

            // Redirecionar minha requisição pra a Ação Index(). 
            // Que é a Ação que vai mostrar na Tela Principal (Index), o meu CRUD de Vendedores.
            //
            // O framework aceita desta forma:
            //return RedirectToAction("Index");
            
            // Colocando o RedirectToAction com o nameof, o "nameof(Index)" melhora a manutenção do meu Sistema. Porque, se amanhã eu mudar o nome do String dessa Ação (Index), eu não vou ter que mudar nada aqui neste comando "return RedirectToAction(namaof(Index))".
            return RedirectToAction(nameof(Index));            
        }
    }
}
