using SalesWebMvc_.Net7.Models; // Para ele enxergar o NOSSO DbContext, que é o SalesWebMvc_Net7Context.     // Embora ele esteja na pasta Data, ele foi declarado com o namespace para a pasta .Models (SalesWebMvc_Net7.Models).
using System.Linq;
using Microsoft.EntityFrameworkCore; // Para a operação Include().
using SalesWebMvc_.Net7.Services.Exceptions; // Para o compilador encontrar a Exceção (classe) NotFoundException e a Exceção DbConcurrencyException. 
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesWebMvc_.Net7.Services
{
    public class SellerService
    {
        private readonly SalesWebMvc_Net7Context _context;

        public SellerService(SalesWebMvc_Net7Context context)
        {
            _context = context;
        }

        //Tentei colocar uma lista<Seller> na Classe ou 1 Atributo Tipo Lista<Seller> não funcionou. Porque?
        //Acho que pelo fato de esta Classe ser registrada (services.AddScoped<SellerService>();) na Startup.cs, o compilador irá entender que esta Classe só pode Operar na(s) suas dependências.
        //List<Seller> list = new List<Seller>();          


        // Por enquanto, o professor deixou esta Operação como sendo uma operação síncrona:
        // Ou seja, ele vai rodar este acesso ao BD (_context.Seller.ToList();), e a Aplicação vai ficar bloqueada esperando é o acesso TERMINAR.
        //
        // Mais prá frente no Curso, nós vamos discutir como transformar estas operações de acesso a dados, em operações ASSÍNCRONAS:
        // Desta forma, aumentando muito a performance da nossa Aplicação.  
        //
        // Este Método (operação) é de um Service. Não de um Controller. 
        // As operações de controlers geralmente são Index(), Contact(), Delete() etc.
        public async Task<List<Seller>> FindAllAsync()
        {
            //List<Seller> list = new List<Seller>();
            //list.AddRange(_context.Seller.ToList());
            //return list;

            // O comando abaixo, substitui tudo acima.  // O comando abaixo, vai acessar minha tabela de dados relacionada à Vendedores (Seller), e converter isto para uma Lista.
            return await _context.Seller.ToListAsync();
        }

        // Método para inserir um NOVO Vendedor (Seller obj) no BD.
        public async Task InsertAsync(Seller obj)
        {
            // A gente tinha colocado um First().
            // Não vamos mais precisar dele. Porque?
            // - Porque agora este meu objeto Seller aqui (obj), vai 
            //   estar devidamente instanciado, já com o Departamento.
            //obj.Department = _context.Department.First();

            // Só adicionar o obj Seller no BD não vai adicionar.
            //
            // A operação Add() do Linq, ela é feita somente em MEMÓRIA.
            //     Então ela NÃO PRECISA ser "Async" (sufixo Async depois de Add).
            _context.Add(obj);

            // Eu preciso também confirmar.
            //
            // Esta operação é que realmente vai acessar o Banco de Dados.
            //    Então nela, é que deve ter a versão "Async" (sufico Async).
            await _context.SaveChangesAsync();
        }

        // Este método vai RETORNAR o Vendedor (Seller).
        // Se o Vendedor não existir, eu vou retornar NULO.
        public async Task<Seller> FindByIdAsync(int id)
        {
            // FirstOrDefault daquele OBJETO obj,
            // cujo (=>) obj.Id seja igual ao id
            // (que eu estou informando como parâmetro).
            //
            // O compilador deu uma mensagem, dizendo que é
            // possível retornar NULO. 
            // ISSO NÃO É ERRO!
            // - Ele só tá avisando que pode ser que não exista
            //   um Vendedor. Daí NULO é um retorno POSSÍVEL.
            //
            //
            //
            // O método Include(), é uma instrução específica do ENTITY FRAMEWORK,
            // prá ele (Entity Framework), fazer o JOIN das tabelas.
            // Com o Include() ANTES do FirstOrDefault(), o framework irá fazer
            // a JOIN das 2 Tabelas:
            // O Departamento (do Vendedor) do Método Include COM o Seller do Método
            // FirstOrDefault().
            // - Com o resultado do Include(), eu vou dar o FirstOrDefaut().
            // - Com esta JUNÇÃO, eu PUDE colocar + os campos "Department" e 
            //   "Department.Id", na View Details.
            // - Porque esta View só trabalha com o model (objeto modelo) do tipo
            //   Seller (Vendedor). Seller é um, Department é outro.
            // - E uma View não aceita 2 @model.  
            // - Este Include() então, faz o que é chamado de "eager loading", que
            //   é "JÁ CARREGAR OUTROS OBJETOS ASSOCIADOS ÀQUELE OBJETO PRINCIPAL".
            //
            //  Não seria melhor criar uma ViewModel?
            //  - Não.
            //    ViewModel é para eu criar um objeto composto (personalizado).
            //    Exemplo, um Seller (Vendedor) com uma Lista<Department>.
            //  - Já o método Include() no context (BD) é para o ENTITY FRAMEWORK
            //    fazer o JOIN de 2 Tabelas:
            //            - da tabela que eu passar no Include() com a Tabela
            //              de um outro Método aplicado depois do Include(), no
            //              Context claro!
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        // Agora vamos implementar o Método Remove.
        // Remover é uma Operação que não precisa retorna NADA.
        // - É só ir la no BD, e remover o elemento.
        // - Por isso é "void".
        public async Task RemoveAsync(int id)
        {
            // Esta implementação é baseada naquele SCAFFOLDING (ferramenta
            // de geração automática da View), que a gente fez lá do
            // Departament.



            // 1° eu vou pegar o OBJETO chamando o
            // "_context.Find passando o id.
            var obj = await _context.Seller.FindAsync(id);

            // Com o OBJETO nas mãos, aí eu chamar o
            // "_context.Seller.Remove(obj);
            //
            // Não tem o comando "return obj;" para retornar 1 Vendedor
            // Remove é uma Ação que eu não preciso RETORNAR NADA.
            _context.Seller.Remove(obj);

            // Removi o objeto do DbSet, ou seja, fiz uma alteração no BD.
            //
            // Agora, eu preciso CONFIRMAR (SaveChanges) esta alteração,
            // para o ENTITY FRAMEWORK efetivá-la, lá no Banco de Dados.
            // Prá fazer isso, eu ou ter que chamar o "_context.SaveChanges()".
            await _context.SaveChangesAsync();
        }

        // O que que vai ser atualizar um OBJETO O TIPO Seller?
        //
        // Update() corresponte a Ação de Edit (página) num CRUD.
        // Tanto que, depois de implementar este Método, eu tenho que verificar SE O LINK PRA 
        // AÇÃO DE Index (de Sellers) ESTÁ CORRETO.
        public async Task UpdateAsync(Seller obj)
        {
            // O que que vai ser atualizar um objeto do Tipo Seller?   

            // - 1ª coisa eu vou testar se o Id deste OBJETO (obj), ele já existe no Banco.
            //
            // Porque, como eu estou ATUALIZANDO, o Id deste OBJETO (obj) já tem que existir.
            // Prá testar isso, eu vou chamar o meu "_context.Seller.Any()"
            //    O Any() serve prá falar SE EXISTE ALGUM REGISTRO NO BANCO DE DADOS, com a condição
            //    que eu colocar nele (nos parâmetros).
            //    - No caso, se existe no Banco. (_context), na tabela Seller. (Seller). AlgumRegistro
            //      x, tal que o Id desteRegistro x seja igual ao Id do obj (do parâmetro). 
            //
            // - O que é o PREDICADO?
            //   É toda a expressão lambda, ou seja, é uma função anônima.
            // - No caso, "x => x.Id == obj.Id" é o PREDICATE.
            // - Então, com o "if", eu estou testando:
            //          SE EXISTE NO BANCO DE DADOS, ALGUM VENDEDOR (Seller) x, CUJO Id SEJA IGUAL AO 
            //          Id DO MEU OBJETO (obj).
                   

            // hasAny = temAlgum
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            //
            //if (_context.Seller.Any(x => x.Id == obj.Id) == false), foi substituído pelo abaixo, com 
            // "!" ANTES. Prá dizer que se NÃO (!) EXISTIR O ELEMENTO NO BD, COM O Id == AO Id DO obj.
            // 
            // Este "if" quer dizer:
            // Se NÃO (!) EXISTIR Qualquer (Any) Vendedor. Vendedor x cujo x.Id seja igual ao obj.Id.
            if (!hasAny)
            {
                // Vou lançar a Exceção NotFoundException, com a MENSAGEM "id not found".
                throw new NotFoundException("id not found");
            }
            // Se passou pelo "if", significa que já existe o objeto lá (no BD).
            // Então, agora, eu vou atualizá-lo:
            // - Eu vou chamar o "_context.Update".
            //
            // Veja o tanto que é simples eu atualizar o objeto usando o ENTITY FRAMEWORK.
            //_context.Update(obj);
            //_context.SaveChanges(); // Para confirmar a Atualização (Update()).



            // Só que tem o seguinte:
            // - Quando eu chamo a Operação de ATUALIZAR NO BANCO DE DADOS (_context.Update() e 
            //   _context.SaveChanges(), o banco de dados pode retornar uma exceção de conflito
            //   de concorrência.
            // - Se este ERRO ocorrer no Banco de Dados, o ENTITY FRAMEWORK vai produzir uma
            //   Exceção chamada "DbUpdateConcurrencyException".
            // 
            // Então, vamos colocar a Operação de ATUALIZAR NO BANCO DE DADOS (_context.Update() e 
            // _context.SaveChanges()), num bloco "try", para TENTAR fazer isso
            // (_context.Update() e _context.SaveChanges()).
            //
            // Antes, vamos comentar então os comandos (operações) de atualizar o BD:
            // _context.Update(obj); E _context.SaveChanges();
            // 
            //
            // E aí, eu vou colocar um bloco "catch", pra capturar uma possível concorrência do
            // Banco de Dados.
            // Que a tal da DbConcurrencyException.
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync(); // Para confirmar a Atualização (Update()).
            }
            catch (DbUpdateConcurrencyException e)
            {                // Então, se acontecer essa Exceção DbUpdateConcurrencyExceptiondo do
                // ENTITY FRAMEWORK, aí eu vou RELANÇAR uma outra exceção em nível de
                // Serviço (pasta Services), que vai ser a MINHA (personalizada)
                // DBConcurrencyException. 
                // - DBConcurrencyException que eu criei.
                //
                // E nessa Exceção, eu vou colocar a mensagem que veio do BANCO DE DADOS
                throw new DBConcurrencyException(e.Message);


                // Perceba o seguinte:
                // Prá nossa aplicação ficar TOP em termos de  CAMADAS, o que eu eu estou
                // fazendo:
                // - Eu estou interceptando uma exceção do Nível de Acesso à Dados, que é a
                //   catch (DbUpdateConcurrencyException.
                // - E eu estou RELANÇANDO esta exceção, só que usando a minha exceção em 
                //   Nível de Serviço: DBConcurrencyException.
                // - Isso aqui é muito importante pra SEGREGAR (isolar) AS CAMADAS:
                //        A minha camada de Serviço, ela não vai propagar uma exceção do Nível
                //        de Acesso A Dados.
                //   Se uma exceção de Nível de Acesso à Dados acontecer, a minha Camada de
                //   Serviço ela lançar uma exceção da Camada dela.
                //        E aí, o meu controlador (que no caso aqui vai ser o SellersController,
                //        ele vai ter que lidar com exceções somente da Camada de Serviço.
                // - Isso é uma forma da gente respeitar aquela arquitetura que nós propusemos a
                //   fazer, que é essa daqui: 
                //    ARQUITETURA GERAL:                                   
                //    View <--> Controller <--> Model (Services / Repositores / Entities).
                //    
                //   A arquitetura Geral:
                //     O Controlador conversa com a camada de Serviço (Services).
                //     Exceções do Nível de Acesso à Dados (Repositories) são capturadaras
                //     (bloco catch) pelo Serviço, e RELANÇADAS na forma de exceções do Serviço
                //     (Serviços) para o controlador.
                //   Ou seja, na nossa arquitetura geral, o controlador não conversa diretamente
                //   com os Repositories (acesso a Dados): Antes dos Repositories, vem os Services.
                //
                //
                //
                //
                //   ARQUITETURA GERAL:                                   
                //   View <--> Controller <--> Model (Services / Repositores / Entities). 
                //
                //   // Veja o esquema no Word desta Aula. 
                //   // Eu entender essa dinâmica para o meu recrutador, vai ser muito positivo
                //   // pra mim.
                //
                //    O Model é dividido em: Serviços - Repositórios / Entidades.
                //   
                //    Veja que na Arquitetura Geral, o Controlador NÃO CONVERSA (não manipula
                //    ou obtém dados) DIRETAMENTE com a Camada de acesso à dados (Repositories):
                //
                //    Eu tenho que implementar:
                //    - Serviços para acessar o BD.
                //    
                //
                //    Serviços (pasta SERVICES) é tudo aquilo que eu usarei para o 
                //    CONTROLADOR possa ter acesso aos dados (REPOSITORIES).
                //
                //    Por motivo de organização, os serviços (classes que realizam operações, no BD
                //    por exemplo), devem ficar na pastinha "Services".
                //    MAS, dependendo do serviço, ele deve ir para outra pasta a que ele faz mais
                //    sentido. Por exemplo, o "SeedingService" que eu criei para povoar o BD em tempo
                //    de execução, ele ficou na pasta "Data" (Dados) junto com o Context (contexto
                //    do BD).
                //
                //    // Como, de acordo com a ARQUITETURA GERAL, em que um Controlador
                //    // não pode acessar / manipular DIRETAMENTE dados em um REPOSITORIES
                //    // (camada/pasta de acesso a dados), ele deve usar um SERVICE (camada/pasta)
                //    // para isso.
                //
                //                
                //    Mas o que são SERVIÇOS?
                //    - É o que terá nesta pasta "Services"?
                //      Esta pasta "Services", representa minha camanda de SERVIÇOS,
                //      chamada "SERVICES" no quadro da ARQUITETURA GERAL.
                //      NELA TERÁ TUDO (classes: de serviços e exceções) para que eu
                //      possa obedecer a (o quadro da) ARQUTETURA GERAL?
                //    - Serviços: classes que eu colocarei métodos (serviços), para
                //      acessar ou manipular os dados das TABELAS (repositories).
                //      Exemplo:
                //         SellerService: classe que terá método (serviços) para
                //                        manipular ou obter dados da TABELA 'Seller'  do BD.
                //    - Exceções:
                //         Classes, que poderão lançar minhas exceções (erros) personalizadas.
                //         MAS, no nível de Serviços (pasta Services, que EU CRIEI).
                //         Nível de Serviços, que eu criei. Não que o framework criou ou que
                //         o ENTITY FRAMEWORK criou.
                //          
                //         Estas exceções de serviços personalizadas, serão relançadas, quando
                //         o ENTITY FRAMEWORK lançar uma exceção da Camada de Dados.
                //         Exemplo:
                //         - Toda vez que eu fizer uma operação de Deleção no BD, o ENTITY
                //           FRAMEWORK poderá lançar a exceção DbUpdateConcurrencyException, sobre
                //           possível concorrencia do BD.
                //         - Ele pode lançar também a NotFoundException.
                //
                //         Aí, eu coloco dentro de um bloco "try", eu vou colocar a operação para
                //         Atualização do BD:
                //         - _contextOuServiço.Update(objeto);
                //         - _contextOuServico.SaveChanges();
                //
                //         1 bloco "catch", e nos () dele eu vou colocar a exceção
                //         DbUpdateConcurrencyException).
                //         - No corpo dele, ie nas {}, eu vou colocar a minha exceção personalizada
                //           DBConcurrencyException (da MINHA camada SERVICES).
                //           Se eu ainda não tiver uma exceção personalizada, eu posso
                //           provisoriamente colocar um "return NotFound();.
                //      
                //         Outro  bloco "catch", e nos () dele eu vou colocar a exceção
                //         NotFoundException.
                //         - No corpo dele, ie nas { }, eu vou colocar a minha exceção personalizada
                //           NotFoundException (da MINHA camada SERVICES).
                //           Se eu ainda não tiver uma exceção personalizada, eu posso
                //           provisoriamente colocar um "return BadRequest();.
                //
                //         O que eu estou fazendo ao colocar os comandos para Atualização do
                //         BD num bloco "try-catch"?
                //         - Eu estou respeitando a ARQUITETURA GERAL. Como?
                //           Como um CONTROLADOR (camada "Controller") não pode ter acesso diretamente a um
                //           REPOSITORIES (subDivisão da Camada "Model"), ele na sequência da ARQUITETURA GERAL
                //           deve ir para um SERVICES (subDivisão da Camada "Model").
                //
                //           Ou seja, ele deve usar uma Classe (que pode ser uma exceção ou serviço),
                //           para quaisquer atividades ligadas à ACESSO à dados (REPOSITORIES):
                //           Seja uma exceção lançada pelo FRAMEWORK, alteração no BD, acesso (leitura)
                //           de um BD.
                //         
                //         - Assim, no exemplo dos comando de atualização no BD dentro de um bloco
                //           "try-catch", eu tento atualizar o BD:
                //             Se der algum erro, este erro será lançado pelo ENTITY FRAMEWORK.
                //             Eu capturo este erro, e RELANÇO (throw) este erro na usando a
                //             mensagem (e.message) que o ENTITY FRAMEWORK lançou na minha exceção
                //             personalizada.
                //      
                //          - Assim eu respeito a ARQUITETURA GERAL:
                //            Um erro (da camada Model/Repositories) não pode ser lançado 
                //            DIRETAMENTE. Capturo ele e o RELANÇO, fazendo com que ele vá para
                //            a camada Model/Services. Prá depois eu lançar. 
                //
                //    
                //
                //
                //
                //
                //
                //     Como eu crio um Serviço (classe que vai ter operações para acesso à Dados)???
                //       Para cada TABELA do BD, eu crio uma Classe (serviço).
                //       Exemplo: Para a TABELA Seller, eu terei a Classe (o serviço) para acessar
                //                os dados da TABELA 'Seller'.
                //                
                //                Obedecendo o PADRÃO de Nomes do FRAMEWORK, este Serviço (Classe)
                //                terá o NOME 'SellerService":
                //                Classe que terá operações para manipular / obter dados da TABELA
                //                'Seller'.
                //
                //     Este serviço, ele vai ter um Tipo Context, ie um BD injetado AUTOMATICAMENTE
                //     nele, toda vez que ele for solicitado (criado).
                //     -Este Context será atribuído a um Atributo "readonly".
                //      Com isso, este meu serviço terá uma dependência com o BD.
                //
                //                              
                //     Num Serviço (Classe) eu implemento os métodos que eu usarei para manipular
                //     (POST) ou obter (GET) dados de uma TABELA do db.
                //
                //     Então, 1 serviço está relacionado com uma TABELA do bd.
                //     
                //     Ou seja, para cada TABELA, eu crio um Serviço (classe só com métodos para
                //     acessar / manipular dados de uma TABELA).            
                //
                //
                //
                //
                //
                //
                // Resumindo então a ARQUITETURA GERAL de um projeto MVC com Entity Framework:
                //
                // View/ Controller/ Model (subdividido em SERVICES, REPOSITORIES, ENTITIES).
                //
                // Views (pasta/camada com as minhas Views ou telas):
                //    São Páginas Razor (arquivos .cshtml), que renderizam uma tela no navegador.
                //
                //    Elas devem ter o mesmo nome do Método (Ação) do Controlador. 
                //
                //   Elas podem ser ao mesmo tempo a requisição (quando renderizada usando o GET
                //   do Controlador) ou resposta (usando o POST no Controlador).
                //   - O FRAMEWORK, através do controlador saberá inteligentemente identificar 
                //     quando se trata de um requisição ou resposta HTTP.
                //
                //
                //
                // 
                // Controllers (pasta/camada com que terá os Controladores):  
                //    Como um CONTROLADOR não pode conversar DIRETAMENTE com a REPOSITORIES,
                //    de acordo com a ARQUITETURA GERAL, um CONTROLADOR que precisar usar dados
                //    da TABELA 'Seller' por exemplo, ele vai ter que conversar com um SERVICE
                //    (classe que tem o métodos, ie os serviços que manipulam/acessam dados)
                //    da TABELA 'Seller', que no caso nós temos é o SellerService.
                //   
                //    E como o CONTROLADOR fará para obter/ler dados, OU lançar um exceção da
                //    Camada REPOSITORIES?
                //    - Através de um Serviço (camada Services), da camada Model/Services.
                //
                //    - injeto no Construtor dele, um serviço (classe com operações em uma TABELA
                //      do BD).
                //      // Lembrando que estas Classes serviços carregam o BD (que tem as TABELAS).
                //      //
                //      // E que eu devo registrar este serviço no Sistema de Injeção de Dependência
                //      // do FRAMEWORK:
                //      // - No ConfigureServices() lá da Startup, coloco
                //      //   "services.AddScoped<ClasseServiço>;".
                //      //
                //      // Um Serviço (Classe) registrado no ConfigureServices() da Startu, será um
                //      // executado mediante uma solicitação HTTP.
                //      // OU seja, quando o usuário faz uma requisição. É um serviço em tempo de
                //      //          execução, com um tempo de vida baseado a cada solicitação.
                //      //
                //      // Um Serviço (Classe) registrado no Configure() da Startup.cs, eu já o
                //      // coloco no parâmetro do Configure().
                //      // Este serviço, vai ser executado toda vez que eu rodar o programa.
                //      // OU seja, é também em tempo de execução, mas toda vez que eu rodo o
                //      //          programa, ele é executado. É o caso do SeedingService.cs, que
                //      //          toda vez que o programa é rodado, ele vai no BD e verifica se
                //      //          há dados nas TABELAS.
                //      //          Se não tiver nenhum dado, ele POVOA (automaticamente) a TABELA,
                //      //          com os dados que estão implementados na SeedingService.
                //
                //    Então, o CONTROLADOR terá injetado no Construtor dele, o serviço que irá fazer
                //    operações (para manipular ou obter dados na TABELA) no BD referente ao nome dele
                //    + o serviço que irá fazer operações na TABELA relacionada à ele.
                //    Exemplo:
                //      O Controlador de Vendedores (SellersController) irá ter no Construtor dele a 
                //      SellerService + a DepartmentService).
                //
                //
                //
                //
                //
                //    Models (modelos), é uma pasta ou camada do meu projeto.
                //    Ela tem sub-pastas: Services, Repositories, Models (Entities).
                //    
                //    A pasta 'Models' ela já vem na estrutura do projeto MVC.
                //    Nela, eu coloco as Entidades (Seller, Department, Aluno etc) do meu negócio.
                //    Que são os modelos do meu negócio.
                //
                //    Na 'Services' (pasta/sub-camada de SERVICES) que terá os serviços para acesso a
                //                  dados (pasta/sub-camada REPOSITORIES) + as exceções lançadas pelo
                //                  ENTITY FRAMEWORK (que cuida do BD).
                //             
                //
                //       Mas o que é um SERVIÇO???
                //       - Serviço é uma Classe, que terá os métodos (serviços) para manipulação / acesso
                //         a dados da minha Aplicação.
                //         Exemplos do métodos desta Classe:
                //         - o Update(), Remove(), Insert(), FindAll() para listar todos os elementos,
                //           Insert() etc.    
                //
                //       - O Nome que eu dou para um Serviço (Classe que eu criar), será de acordo com
                //         o Nome da TABELA. 
                //           Exemplos:
                //           - "SellerService" é o nome do serviço (classe) que tem método para acessar
                //                  manipular dados da TABELA) 'Seller';
                //           - "DepartmentService", é o nome do serviço para manipular / obter dados da
                //                  TABELA 'Department'.
                //                  (repositories) 'Seller' e 'Department'. 
                //
                //       Um serviço NÃO é para acessar Membros das Classes (Entidades).
                //       É para acessar uma TABELA do BD. 
                //   
                //
                //
                //
                //
                //
                //
                //
                //
                //
                // Foi o que fizemos:
                //  - Eu fiz 2 EXCEÇÕES:
                //       - Cada uma delas, será RELANÇADA com a mensagem de erro de uma exceção da Camada de
                //         REPOSITORIES (Model/Repositoris), mensagem esta lança pelo ENTITY FRAMEWORK.
                //         Num bloco "try-catch".                
                //  - Eu fiz 1 Serviço (SeedingService) pra popular o BD.
                //  - O FRAMEWORK tem 1 Serviço (services.AddDbContext<>) que nos <> eu informo qual será o
                //    meu Context (BD) que eu vou injetar AUTOMATICAMENTE em outras Classes (Serviços).       
                //  - Eu fiz 1 Serviço (DepartmentService), que depende (injeção de Dependência)
                //       do nosso Context (que usa Serviços do FRAMEWORK para uma sessão com o BD)
                //       para Listar os Departamentos.
                //  - Eu fiz este Serviço aqui (SellerService), que também depende do nosso Context
                //       para as operações relacionadas à Entidade Seller (Vendedor).
                //       Com as operações:
                //       - FindAll (para listar todos os Sellers),
                //       - FindById (para mostrar 1 Seller, baseado num Id),
                //       - Insert (Inserir 1 new Seller no BD).
                //       - Remove (Remover 1 Seller do BD).
                //       - Update (Atualiza os dados de 1 Vendedor).
                //   
                //   Assim, para um projeto bem feito (que segue uma arquitetura geral), um
                //   Controlador, por exemplo o SellersController, nunca terá acesso diretamente
                //   a um repositorie (CAMADA que acessa Dados).
                //   UM controlador deverá, através da CAMADA de Serviços, acessar a Camada de
                //   Acesso à Dados (repositorie).
                //     - Ele é uma Classe que herda da Controller (superClasse).
                //     - Ele é uma Classe então com suporte à View (página Razor).
                //     - Com ele eu não devo acessar ou manipular um Banco de Dados, DIRETAMENTE:
                //           Mas com os Serviços injetados nele (no controlador) para isso.
                //     - Assim, as Ações (requisições) do usuário, ele (controlador) vai usar os
                //       serviços injetados nele, e com o retorno desses serviços - como um
                //       controlador HERDA de uma Classe que tem suporte a View, ele também
                //       terá suporte a View - geralmente ele retornará uma View (pagina .cshtml)
                //       para o usuário com o mesmo nome da requisiçãoDoUsuárioNoNavegador
                //       (Ação), contendo o objeto retornado da operação (ação / método do
                //       controlador).               
            }
        }
    }
}
