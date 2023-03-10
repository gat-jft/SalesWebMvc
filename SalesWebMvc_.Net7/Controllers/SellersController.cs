using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using SalesWebMvc_.Net7.Models; // Para enxergar o "Seller".
using SalesWebMvc_.Net7.Models.ViewModels; // Para ele exergar a Classe "SellerFormViewModel". Usada no Crete(), com [GET] 
using SalesWebMvc_.Net7.Services;
using SalesWebMvc_.Net7.Services.Exceptions; // Para ele importar (ou enxergar) o NotFoundException e o DbConcurrencyException
using System.Diagnostics; // Para ele reconhecer a Classe Activity, que eu chamei na Ação Error.
using System.Linq;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SellersController : Controller
    {
        // Pra que o Index() chame o FindAll() lá do SellerService, vamos ter que declarar uma dependência (Property, não injeção de dependência que é outra coisa) com o SellerService.
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;


        // Vamos fazer o nosso Construtor pra ele injetar as dependências (atributos
        // preenchidos automaticamente).
        //
        // E quando eu sei que é para criar (instanciar) um objeto SellerService?
        // - É quando o usuário requisita (digitando) no navegador o NOME do Controlador.
        //   Por exemplo, se ele digitar depois do https//localhost/ "Sellers", 
        //   o framework já cria (instancia) um SellersController.
        //   Pois ele já sabe porque a sequência de solicitação é:
        //   Controlador/Ação, ou seja Controlador, DEPOIS a AçãoDesteControlador.
        //
        // - E como o framework também é fortemente por padrão de Nomes, ele irá procurar
        //   o Controlador "Sellers" (que é prefixo), porque "Sellers" é o prefixo de 
        //   do Nome "SellersController".
        //
        // - Então, será criada um SellersController, e associado à este objeto.
        //   virão as Classes (os serviços) SellerService e DepartmentService.
        public SellersController
            (SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
        // - NOTE que eles retornam sempre 1 IActionResult(View, RedirectToAction etc).
        // - Não retornam int, Seller etc.
        // 
        // Porque?
        // - Porque um IActionResult já é por exemplo uma View (tela) já montada.

        // Este método Index() vai ter que chamar a operação FindAll(), lá do SellerService.
        // Antes  disso, temos que declarar uma dependência para o SellerService (que é o _sellerService). Depois, devemos fazer o Construtor (public SellerSevice) deste Controller, prá ele INJETAR A DEPENDÊNCIA do serviço (SellerService).
        public async Task<IActionResult> Index()
        {
            // Poderia ser declarado no lugar do Tipo var (genérico), o Tipo List<Seller>.
            // A operação FindAll() do nosso serviço SellerService, vai me retornar uma Lista de Seller.
            //
            // A indicação "await" é prá ele (compilador) ESPERAR A RESPOSTA dessa chamada (_sellerService.FindAllAsync();) que é ASSÍNCRONA.  
            var list = await _sellerService.FindAllAsync();

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


        //Ação (com o Método GET DO HTTP) para criar um Novo Vendedor.
        public async Task<IActionResult> Create()
        {
            // 1°) Carregar (buscar do BD para cá) os Departamentos:
            //       FindAll() é o método do serviço DepartmentService,
            //       que busca do BD Todos os Departamentos.
            var departments = await _departmentService.FindAllAsync();

            // 2°) Instanciar um OBJETO do nosso ViewModel:
            //     Este ViewModel (SellerFormViewModel) nosso, ele tem 2 dados
            //     - o Departaments (Departamentos), e,
            //     - o Seller (Vendedor).
            //         No Departments, eu já vou INICIAR com esta
            //         Listinha de Departamentos, que nós acabamos de
            //         buscar (carregar) do BD.
            var viewModel = new SellerFormViewModel { Departments = departments };

            // Como esta Ação é um MÉTODO GET DO HTTP, ela simplesmente retorna 1 View VAZIA.
            // Nós SEMPRE vamos usar este PADRÃO aqui:
            // - Se eu tiver uma Ação que altera alguma coisa no sistema (ou seja, usa o MÉTO POST DO HTTP), como o caso da Create(Seller seller):
            //   Eu tenho que ter 2 Ações com o mesmo nome:
            //   - Uma sem argumento,
            //   - E uma sobrecarga dela, isto é, com argumento.
            //     O argumento é o dado que vai ser inserido OU modificado.
            //   - É este exemplo aqui:
            //        Create() só retorna a View vazia.
            //        Create(Seller seller) é o tipo do dado que vai ser Alterado OU Inserido no sistema.
            //   - Se eu não fizer neste padrão, dá ERRO ao executar.
            //      Então, prá Cada Ação POST DO HTML, eu tenho que criar um GET. 
            //   - O raciocínio é o seguinte:
            //      Eu posso estar numa Ação (View no navegador) que tenha FORMULÁRIO DE CADASTRO PARA INSERIR UM NOVO Funcionário no BD.
            //      Neste formulário tem um botão com a <input> "submit", quando eu clicar nele.
            //        Daí, eu desista preencher este novo funcionário.
            //        - Eu posso clicar no Botão, sem com o formulário VAZIO.
            //          Como ele está VAZIO, a Ação será a Ação GET DO HTTP.
            //        - Creio que o framework INTERPRETARÁ que se trata de chamar a AÇÃO COM O MÉTODO GET, pois eu estou enviando um formulário VAZIO.
            //          Daí, ele não faz nada no BD, e simplesmente retorna a View VAZIA.
            //          Ou seja, retorna um formulário vazio.
            //          Por isso este "return View()"
            //        - Se não tivesse esta AÇÃO SEM ARGUMENTO (ie, MÉTODO GET DO HTTP), no BD seria INSERIDA 1 Linha VAZIA.
            //          E meu BD poderia ficar cheio espacos VAZIOS (sem elementos).                    

            // 3°) Feito isso (instanciando um SellerFormViewModel com uma
            //     lista de Departamentos, mas SEM o Seller), eu vou passar
            //     este OBJETO "viewModel", pra minha View (Create, pois é este
            //     Método aqui.
            //     Vou passar com este comando abaixo: 
            //     - return View(viewModel);
            //
            // Agora, a minha TELA DE CADASTRO DE 1 VENDEDOR, quando ela for
            // acionada pela 1ª VEZ, ela já vai receber este OBJETO
            // "viewModel" (do Tipo SellerFormViewModel).
            // - Com o Vendedor em branco, e
            // - com os Departamentos POPULADOS. 
            return View(viewModel);
        }

        // Quando o usuário clica num LINK ou BOTÃO para a Ação Create, ele
        // (framework) entende que é para mostrar a Ação Create [GET] anterior.
        // Ou seja, mostrar um SellerFormViewModel, com:
        // - Vendedor em branco, e,
        // - Lista<Departments> populada.
        //
        // Aí, quando eu preencho os dados das LABELs da View Create, e clico no
        // botão, que tem o <submit>.
        // <submit> quer dizer "enviar o FORMULÁRIO para a View Create"
        // (Ação Create no nosso>.
        // O FRAMEWORK já entende que é para que usar o
        // Create(Vendedor vendedor), porque <submit> é para enviar os
        // dados (do formulário).
        // _ E a Ação (Método) que que recebe DADOS (argumento) de um
        //   Vendedor é a Create(Vendedor vend), que tem o Método [POST].
        //
        //
        //
        // RESUMINDO:
        // Quando eu tenho 1 Ação (Método que criará um View com o mesmo
        // Nome) com sobrecarga, a cada vez que eu entrar nesa View (Açã),
        // o FRAMEWORK sabe que é para a Ação com o Método [GET], que
        // mostra um objeto VAZIO, que no nosso caso é um SellerFormViewModel
        // com Vendedor + List<Departamentos>.
        //
        // Mas quando eu clico no botão com <submit>, o FRAMEWORK a Ação que
        // recebe dados (no caso um Vendedor) é a Create(Seller seller)
        // [POST].
        // Ou melhor dizendo, [HttpPost]. Que é o Método que altera algo no
        // sistema, via HTTP (internet).
        // 
        // 
        //
        //
        //
        //
        //
        // Como a gente mudou o Modelo da nossa View Create, que antes era 
        // Seller, agora é SellerFormViewModel, a gente espera então que na
        // Ação Create do [POST], que é a Ação que vai receber o FORMULÁRIO,
        // eu vou ter que atualizar esta Ação:
        // - Mudando o argumento da função para SellerFormViewModel.
        // 
        // Aqui no framework, ISSO NÃO É NECESSÁRIO!!!!
        // Porque?
        //
        // - O FRAMEWORK é inteligente o suficiente pra saber que o ViewModel 
        // (SellerFormViewModel) vindo do Create [GET], SERVE pra montar um
        // objeto do tipo Seller.
        // Porque lá no nosso Model Seller nós acrescentamos o Atributo
        // DepartmentId.
        // - Como nós usamos o nome CERTINHO "DepartmentId" (Department seguido
        //   de Id), ele automaticamente sabe que ele vai ter que pegar aquele
        //   Código de Departamento que eu enviei no formulário (na View
        //   Create, de SellersController) e instanciar o Departamento dele
        //   (do Seller).
        //
        // 
        // Prá que eu receba este objeto da requisição, e instancie este Vendedor, basta eu colocar ele (este objeto) aqui como parâmetro do Método.
        [HttpPost]                       // Com esta ANNOTATION [HttpPost], eu estou indicando que esta minha Ação (Método                       // ANNOTATION [HttpPost].   ANNOTATION é uma Classe (tipo). Esta ANNOTATION [HttpPost], ele Identifica uma ação (este método Create) é uma Ação de POST e não de GET.         // , que será uma View / Página) que suporta o método HTTP POST.     POST é quando eu tenho uma Operação (Ação) que altera algum dado, como por exemplo, qualquer alteração no BD, seja ela inclusão / deleção etc.      // Então, ela só pode ser usada em Método. NÃO POSSO COLOCAR NUMA class.     // Para uma Ação que usa o método POST.       // Para toda ação POST que eu criar, tenho que ter uma Ação GET, senão não funciona, não renderiza a página.   Por isso que tive que ter o Método anterior, com o mesmo Nome, e SEM o POST, indicando que é GET.    MAS sem parâmetro.     deste Método Então, este  A Ação anterior de mesmo nome é obrigatória, senão não gera a página.
        [ValidateAntiForgeryToken]       // Validar token antifalsificação.     // ANNOTATION [ValidateAntiForgeryToken] é prá prevenir que a minha aplicação sofra ataque CSRF.        // Este tipo de ataque é quando alguém aproveita a minha SESSÃO de autenticação, e envia dados maliciosos aproveitando a minha autenticação.       // Para mais detalhes sobre isso, ver o link do material de apoio.       // Esta ANNOTATION [ValidateAntiForgeryToken], ela especifica que a classe ou método ao qual este atributo é aplicado valida o token anti-falsificação. Se o token antifalsificação não estiver disponível ou se o token for inválido, a validação falhará e o método de ação não será executado.      // Observações:    Este atributo ajuda na defesa contra a falsificação de solicitação entre sites. Isso não impedirá outros ataques de falsificação ou adulteração.           // Token é: Token é um dispositivo eletrônico gerador de senhas, geralmente sem conexão física com o computador, podendo também, em algumas versões, ser conectado a uma porta USB (porta de pendrives). Conectado a uma porta USB, no caso é um pendrive com um programa gerador de senhas eletrônicas (programa token).
        public async Task<IActionResult> Create(Seller seller)
        {
            // Este teste serve prá testar se o modelo foi VALIDADO.
            // Se não (!) for válido, eu vou retornar a minha View (Create), com o objeto (seller).
            if (!ModelState.IsValid) // Isto testa todas as validações (Annotations) que eu coloquei em cima de cada Atributo da Classe (Entidade ou model, lá na pastinha Models) Seller.
            {
                var departments = await _departmentService.FindAllAsync(); // Carrego os Departamentos
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; // Crio um objeto SellerFormViewModel, aonde o Atributo Seller dele receberá o seller, e o Atributo Departments dele receberá a listinha de departamentos que eu carreguei.
                return View(viewModel);      // simplesmente vou retornar a minha View (que é o Create), 
                                             // repassando o meu objeto:  “Volta prá lá e acaba de consertar 
                                             // (corrigir)”.
                                             //      Isso vai ficar acontecendo, enquanto o USUÁRIO não 
                                             //       preencher DIREITINHO o formulário.
            }


            // Inseriu + este Vendedor no BD.
            await _sellerService.InsertAsync(seller);

            // Redirecionar minha requisição pra a Ação Index(). 
            // Que é a Ação que vai mostrar na Tela Principal (Index), o meu CRUD de Vendedores.
            //
            // O framework aceita desta forma:
            //return RedirectToAction("Index");

            // Colocando o RedirectToAction com o nameof, o "nameof(Index)" melhora a manutenção do meu Sistema. Porque, se amanhã eu mudar o nome do String dessa Ação (Index), eu não vou ter que mudar nada aqui neste comando "return RedirectToAction(namaof(Index))".
            return RedirectToAction(nameof(Index));
        }

        // Este Delete, é simplesmente prá Abrir uma Tela de Confirmação:
        // Perguntando: "Tem certeza que você quer Deletar?".
        //
        // Ela não é ainda prá Deletar de fato: É só prá confirmar.
        // Depois a gente vai ter (criar abaixo) o Delete() [POST],
        // que aí sim a gente vai DELETAR do BD.
        //
        // Esta Ação vai receber um int que é OPCIONAL (?), que é o id.
        public async Task<IActionResult> Delete(int? id)
        {
            // Vamos fazer aqui a implementação que também é baseada lá no
            // SCAFFOLDING (geração automática de uma View).
            // - Questionamento:
            //   POSSO CRIAR UM Controlador COM SCAFFOLDING TAMBÉM???

            // 1° eu vou testar se este "id" é NULO.
            // - Se VERDADEIRO, significa que a requisição foi feita de
            //   uma forma indevida.
            if (id == null)
            {
                // Este NotFound(), ele instancia uma resposta básica.
                // Depois (mais prá frente no Curso), nós vamos personalizar
                // este retorno com uma PÁGINA DE ERRO.
                //return NotFound();


                // Como nós já personalizamos nossa página de erro (ErrorViewModel), nós não
                // vamos usar o "return NotFound();" ou o "return BadRequest();" mais.
                //
                // No lugar destes comandos, nos vamos redirecionar para a Ação Error
                // (nameof(Error).
                // - E esta Ação Error, ela recebe um argumento. Prá passar este argumento,
                //   eu vou criar aqui um objeto ANÔNIMO, 
                //
                // - Nos "" do objeto ANÔNIMO, eu vou colocar a mensagem que eu quiser.
                //       No caso aqui de Deletar (Ação Delete) e o id não foi fornecido
                //       (provided), eu vou colocar "Id not provided".
                //
                // É assim que eu vou Redirecionar pra minha Página (Action) de Erro.
                // Posso dar um CTRL C, porque eu vou usar bastante prá colar.                
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // Próximo passo então, é pegar este objeto que eu estou mandando
            // DELETAR.
            //
            // Vou pegar este OBJETO, chamando o serviço "_sellerService" .FindById()
            // passando este "id.Value".
            // - "Value" porque ele (id) é um NULLABLE: 
            //           Ele é um OBJETO opcional.
            // - Prá eu pegar o VALOR dele caso existe, tem que ser o
            //   "Value".
            // - FindById(int id), retorna o PrimeiroOuDefault Seller (Vendedor).o
            var obj = await _sellerService.FindByIdAsync(id.Value);

            // Com o comando ACIMA, então, Busquei do Banco de Dados.
            //
            // Agora, esse "id" que eu passei, pode ser um "id" que não
            // existe.
            // - Se ele não existir, o meu método FindById() retorna NULO.
            // - Aí, a variável "obj" ACIMA terá o valor NULL.
            if (obj == null)
            {
                // Substituído pelo Redirecionamento para a Página de Erro abaixo.
                //return NotFound();

                // Como eu mandei procurar o Id (FindById()), e o Id não existia, então a
                // mensagem é "Id not found".
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // Agora se tudo deu certo, eu vou mandar ESTE meu Método
            // "Delete()" retornar uma View (tela),
            // passando este obj (Seller / Vendedor) como argumento:
            //
            // Esta View eu ainda criar na pastinha Views/Sellers.
            // Ela terá que ter o mesmo nome deste Método aqui, claro!
            // 
            // Se o objeto Seller existir, retorna a View (tela) de 1 Seller (Vendedor).
            return View(obj);
        }

        // Vamos criar agora uma outra Ação Delete, só que agora vai ser POST.
        [HttpPost]                       // Com esta ANNOTATION [HttpPost], eu estou indicando que esta minha Ação (Método)                       // ANNOTATION [HttpPost].   ANNOTATION é uma Classe (tipo). Esta ANNOTATION [HttpPost], ele Identifica uma ação (este método Delete) é uma Ação de POST e não de GET.         // que será uma View / Página) que suporta o método HTTP POST.     POST é quando eu tenho uma Operação (Ação) que altera algum dado, como por exemplo, qualquer alteração no BD, seja ela inclusão / deleção etc.      // Então, ela só pode ser usada em Método. NÃO POSSO COLOCAR NUMA class.     // Para uma Ação que usa o método POST.       // Para toda ação POST que eu criar, tenho que ter uma Ação GET, senão não funciona, não renderiza a página.   Por isso que tive que ter o Método anterior, com o mesmo Nome, e SEM o POST, indicando que é GET.    MAS sem parâmetro.     deste Método Então, este  A Ação anterior de mesmo nome é obrigatória, senão não gera a página.
        [ValidateAntiForgeryToken]       // Validar token antifalsificação.     // ANNOTATION [ValidateAntiForgeryToken] é prá prevenir que a minha aplicação sofra ataque CSRF.        // Este tipo de ataque é quando alguém aproveita a minha SESSÃO de autenticação, e envia dados maliciosos aproveitando a minha autenticação.       // Para mais detalhes sobre isso, ver o link do material de apoio.       // Esta ANNOTATION [ValidateAntiForgeryToken], ela especifica que a classe ou método ao qual este atributo é aplicado valida o token anti-falsificação. Se o token antifalsificação não estiver disponível ou se o token for inválido, a validação falhará e o método de ação não será executado.      // Observações:    Este atributo ajuda na defesa contra a falsificação de solicitação entre sites. Isso não impedirá outros ataques de falsificação ou adulteração.           // Token é: Token é um dispositivo eletrônico gerador de senhas, geralmente sem conexão física com o computador, podendo também, em algumas versões, ser conectado a uma porta USB (porta de pendrives). Conectado a uma porta USB, no caso é um pendrive com um programa gerador de senhas eletrônicas (programa token).
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);

                // Removi e Vendedor - no comando anterior -, agora eu vou redirecionar prá tela inicial (View Index) de listagem de vendedores do meu CRUD. 
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }


        // Vamos criar a Ação Details(), no método GET.
        // Esta ação vai receber um id OPCIONAL.
        //
        // A lógica dele, vai ser muito parecida com a do Delete() GET.
        // Porque?
        // - Eu vou ter que ver se meu id é NULO.
        // - Eu vou ter que buscar o meu objeto.
        //      Se ele for NULO também, eu vou dar o NotFound().
        // - Depois, eu vou retornar o objeto.
        // 
        // Então, vamos copiar de lá prá cá.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                // Foi substituído pelo RedirectToActio(nameof(Error, ...) abaixo;
                //return NotFound();


                // Se o id não foi fornecido na requisição, eu vou Redirecionar pro erro
                // "id not provided".
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                // Substituído pelo RedirectToAction(nameof(Error), new ...) abaixo:
                //return NotFound();

                // Se eu buscar o id e ele não existir, eu vou Redirecionar pro ERRO
                // "Id not found";.
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // No final, passando por tudo acima, se o objeto Seller (Vendedor) existir,
            // retorna a View (tela) de 1 Seller (Vendedor)
            return View(obj);
        }

        // Ação Edit, com o método GET (não altera nada no sistema).
        //
        // Esta Ação Edit, ela serve prá abrir a telinha pra editar o nosso Vendedor
        //
        // No caso, ela recebe um Id como argumento, que pode ser OPCIONAL.
        // - Esse OPCIONAL (?), na verdade é só pra evitar de acontecer algum erro 
        //   execução.
        // - Na verdade, ele é OBRIGATÓRIO.
        // - Inclusive, eu já comecei a implementação testando se este Id for igual
        //   a NULO.
        public async Task<IActionResult> Edit(int? id)
        {
            // Se o id for NULO, retorna só uma mensagem de "Não Encontrado"
            if (id == null)
            {
                // Substituído pelo RedirectToAction da mensagem de ERRO, abaixo
                //return NotFound();

                //
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // Testar se este "id" realmente existe no Banco de dados.
            //
            // Para verificar, eu vou usar o FindById(int id) do serviço
            // SellerService injetado aqui no construtor, para a dependência
            // (_sellerService).
            //
            // No FindById() o (int) id é obrigatório.
            // Aqui neste Edit, o (int?) id que ele está usando é OPCIONAL(?).
            // - Aí, o compilador não tava aceitando, dizendo que não tem como converter
            //   um int? para int.
            // - Mas, se eu colocar o ".Value" depois da variável int, eu estou dizendo que
            //   é para pegar o VALOR do id.
            //   Ou seja, ele é obrigatório.
            //   Aí, não dá mais conflito (erro).
            //
            // Eu poderia fazer este teste com 1 "if" apenas, sem criar uma variável, assim:
            //    if (_sellerService.FindById(id.Value) { return NotFound(); }.
            //    
            // Ou posso também, prá fazer isso, eu fazer o "var obj = ...".
            // Porque dessa forma foi melhor?
            // - Porque o FindById retorna um objeto (tipo Seller). E com este objeto em mãos, 
            //   ele vai para uma variável tipo var, que eu dei o nome de "obj".
            //   E com este objeto em mãos, ele pode ser retornado para uma View,
            //   no "return View();"
            // - Usando "if", eu  teria que criar a variável "obj" fora dele, pois o Seller
            //   capturado dentro dele, quando o "if" fosse fechado, ela perderia o escopo.
            //   
            // Na variável obj, pode ter um objeto (tipo Seller) ou NULO (quando não há objeto).
            // Se for NULO, retorna o Método NotFound().
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                // Este NotFound() é provisório.
                // Foi substituído pelo RedirectToAction(Error) abaixo:
                // Na proxima aula, eu vou aprender a retornar uma página personalizada de ERRO.
                //return NotFound();

                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // Testei se o id não existe (1° if). Testei se o id é NULO (2° if).
            // Se tudo isso passar, é sinal que na obj tem um objeto (Seller).
            // Agora sim, eu vou abrir a minha tela de edição.
            //
            // Prá abrir essa Tela de Edição, eu tenho que carregar os Departamentos, prá
            // POVOAR a minha caixinha de Seleção. 
            // Para carregar os Departamentos, eu vou chamar o serviço _departmentService.
            List<Department> departments = await _departmentService.FindAllAsync();

            // Agora eu vou criar objeto (chamado "viewModel") do tipo SellerFormViewModel.
            //
            // E aí, eu já vou passar os dados prá ele. Quem vai ser os dados?
            // - O "Seller", eu vou iniciar ele com o "obj", que é este objeto que nós buscamos
            //   do Banco de Dados, com a chamada "_sellerService.FindById(id.Value);".
            //   Como eu estou fazendo uma EDIÇÃO, eu vou preencher meu formulário com os
            //   dados do objeto existente;
            // - O "Departments", vai ser a Listinha "departments" que eu acabei de carregar.
            //
            // Como ele não tem um Construtor para receber os dados nos (), eu vou passar os
            // dados diretamente (já começando a passar os dados { }).
            // Quando eu não tenho () para passar os dados, eu uso as {}.
            var viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };

            // Instanciamos o nosso ViewModel, então agora vou RETORNAR uma View, passando este
            // ViewModel como argumento:
            return View(viewModel);
        }

        // Vamos criar a Ação Edit, para o método POST.
        // Neste caso, a Ação vai receber o id, e também o objeto Seller.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            // Este teste serve prá testar se o modelo foi VALIDADO.
            // Se não (!) for válido, eu vou retornar a minha View (Edit), com o objeto.
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync(); // Carrego os Departamentos
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments }; // Crio um objeto SellerFormViewModel, aonde o Atributo Seller dele receberá o seller, e o Atributo Departments dele receberá a listinha de departamentos que eu carreguei.
                return View(viewModel);      // simplesmente vou retornar a minha View (que é o Create), 
                                             // repassando o meu objeto:  “Volta prá lá e acaba de consertar 
                                             // (corrigir)”.
                                             //      Isso vai ficar acontecendo, enquanto o USUÁRIO não 
                                             //       preencher DIREITINHO o formulário.
            }



            // Como que nós vamos implementar esta Ação?

            // 1ª coisa:
            //    Eu vou testar se o "id", que veio aqui no parâmetro do Método
            //    for diferente do "Id do seller" também do parâmetro, significa
            //    que alguma coisa tá errada.
            //    Ou seja, o Id do Vendedor que eu estou atualizando, não pode ser
            //             diferente do id da URL da requisição.
            if (id != seller.Id)
            {
                // Provisório.
                // Se isso acontecer, por enquanto eu vou chamar o BadRequest().
                //
                // Como já temos uma pagina de erro, agora, quando for um BadRequest,
                // eu vou Redirecionar prá Página de Erro (Ação Error).
                //return BadRequest();

                // Eu mandei Editar um Vendedor. 
                // Só que o id (int id) que que passei na URL era diferente do id do 
                // Seller.
                //
                // Ai, neste caso, eu vou dar a mensagem que "os id´s não correspondem"
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            // Se passar pelo "if" acima, significa que tá OK.
            // Aí, eu vou chamar o meu "_sellerService.Update()" do meu "seller"
            //_sellerService.Update(seller);

            // Feito a ATUALIZAÇÃO do Vendedor (comando anterior), eu vou
            // Redirecionar a minha REQUISIÇÃO (ação) para a página inicial
            // do CRUD, que é a Index.
            //
            // Então eu vou chamar aqui "return RedirectToAction", e aí eu vou
            // chamar "(nameof(Index))";
            //return RedirectToAction(nameof(Index));

            // Só que aí é aquela coisa:
            // Esta chamada do Update(seller), ela pode lançar EXCEÇÕES a nível da
            // camada de Dados:
            // - Ela pode lançar aqui tanto um NotFoudException quanto um DbConcurrencyException.
            //
            // Então, eu vou fazer o seguinte:
            // Aqui no controlador, esta chamada ("_sellerService.Update(seller); E
            // "return RedirectToAction(nameof(Index));") dentro de um "try".
            //
            // Para isso, COMENTEI (//) esses 2 comandos.
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));

            }
            // Em seguida, vou fazer os meu "catch´s"
            catch (ApplicationException e)
            {
                // Provisóriamente eu vou retornar um NotFound().
                // Como é provisoriamente, eu uso o return.
                // 
                // Depois que eu tiver implementado a Ação com a Página de Erro, eu vou
                // retonar a página de erro.
                //
                // Quando eu já sei qual é a Exceção, eu faço uso do lançamento
                // da Exceção: uso o "throw new Exceção("");", ao invés do "return NotFound();".
                // 
                // NotFound() é um Método SemAção (Tem a ANNOTATION [NonAction], acima dele).
                // - Ele retorna um NotFoundResult().
                //
                // Substituído pelo
                // return RedirectToAction(nameof(Error, new { ... }) abaixo.
                //return NotFound();

                // No caso aqui, eu estou tratando a possível exceção NotFoundException.
                // Como é uma EXCEÇÃO, e a Exceção ela carrega uma mensagem, o que que
                // eu vou fazer?
                // - Eu vou colocar na frente da DECLARAÇÃO Exceção nos (), um apelido prá
                //   ela, por exemplo o apelido e.
                //
                // - E na hora de Redirecitonar prá minha página de Erro, a mensagem
                //   que eu vou colocar vai ser a mensagem da Exceção (e.Message).
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

            // Eu posso tirar os 2 "catch" e deixar só 1.
            // Mas para isso, eu tenho que colocar nos () deste "catch" o 
            // ApplicationException.
            //
            // Porque que isso funciona?
            // - Porque o ApplicationException é um SUPERtipo da 2 Exceções
            //   (NotFoundException e DbConcurrencyException).
            //   Então, por meio de UPCASTING, as 2 Exceções podem casar com 
            //   ApplicationException.
            //
            //
            // Ficando assim:
            //catch (ApplicationException e)
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message });
            //}
        }

        // Ação Error, recebendo um string message como argumento.
        //
        // Esta Ação de Error ela não precisa ser ASSÍNCRONA, porque ela não tem NENHUM acesso a dados.
        //      Ela vai direto retornar a View.
        public IActionResult Error(string message)
        {
            // O que que esta Ação vai fazer?

            // 1ª coisa:
            // Eu vou instanciar aqui um ViewModel.
            // - Como ele não tem Construtor, isto é (), eu vou usar as {}
            //     "var" é um tipo genérico: posso usar ele prá tudo.
            //
            // Eu poderia no lugar do "var", colocar o tipo ErrorViewModel. 
            var viewModel = new ErrorViewModel
            {
                // Quando é instanciação usando {}, os Atributos são 
                // separados por ','.
                // - Se eu usar ';' dá erro.
                Message = message,

                // E quem que vai ser o RequestId da minha requisição?
                //
                // Aí, eu vou usar um MACETINHO interno do FRAMEWORK, prá eu
                // pegar (atribuir à uma variável) o Id interno da requisição:
                // No caso a variável será o Atributo Request Id da Classe ErrorViewModel,
                // que é o tipo deste objeto "var viewModel".
                // 
                // 
                //
                // Eu vou pegar (atribuir à uma variável, no caso o Atributo RequestId da
                // Classe ErrorViewModel) assim: 
                // Activity.Current?.Id ?? HttpContext.TraceIdentifier.
                //   - Activity: Classe que pertence ao namespace System.Diagnostics;
                //     Currenty?.Id:
                //        Esse Currenty ele é OPCIONAL, por causa da '?';
                //        Se ele (Currenty) for nulo, aí eu vou usar o ...
                //   - operador "??"
                //        OPERADOR de COALESCÊNCIA NULA.                               
                //   - HttpContext.TraceIdentifier:
                //        Aí (se ele for nulo), eu vou dizer então, que eu vou querer
                //        então no lugar o objeto HttpContext.TraceIdentifier. 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            // Criamos nosso objeto viewModel (no comando anterior).
            //
            // Agor, eu vou mandar retornar 1 View (a Error, claro!), passando este objeto "viewModel"
            // como argumento. // Que é o arquivo Error.cshtml, na pastinha Views/Shared.
            return View(viewModel);

        }
    }
}
