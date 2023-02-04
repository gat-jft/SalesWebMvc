using SalesWebMvc_.Net7.Models; // Para ele enxergar o NOSSO DbContext, que é o SalesWebMvc_Net7Context.     // Embora ele esteja na pasta Data, ele foi declarado com o namespace para a pasta .Models (SalesWebMvc_Net7.Models).
using System.Linq;
using Microsoft.EntityFrameworkCore; // Para a operação Include().
using SalesWebMvc_.Net7.Services.Exceptions; // Para o compilador encontrar a Exceção (classe) NotFoundException e a Exceção DbConcurrencyException. 

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
        public List<Seller> FindAll()
        {
            //List<Seller> list = new List<Seller>();
            //list.AddRange(_context.Seller.ToList());
            //return list;

            // O comando abaixo, substitui tudo acima.  // O comando abaixo, vai acessar minha tabela de dados relacionada à Vendedores (Seller), e converter isto para uma Lista.
            return _context.Seller.ToList();
        }

        // Método para inserir um NOVO Vendedor (Seller obj) no BD.
        public void Insert(Seller obj)
        {
            // A gente tinha colocado um First().
            // Não vamos mais precisar dele. Porque?
            // - Porque agora este meu objeto Seller aqui (obj), vai 
            //   estar devidamente instanciado, já com o Departamento.
            //obj.Department = _context.Department.First();

            // Só adicionar o obj Seller no BD não vai adicionar.
            _context.Add(obj);

            // Eu preciso também confirmar.
            _context.SaveChanges();
        }

        // Este método vai RETORNAR o Vendedor (Seller).
        // Se o Vendedor não existir, eu vou retornar NULO.
        public Seller FindById(int id)
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
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        // Agora vamos implementar o Método Remove.
        // Remover é uma Operação que não precisa retorna NADA.
        // - É só ir la no BD, e remover o elemento.
        // - Por isso é "void".
        public void Remove(int id)
        {
            // Esta implementação é baseada naquele SCAFFOLDING (ferramenta
            // de geração automática da View), que a gente fez lá do
            // Departament.



            // 1° eu vou pegar o OBJETO chamando o
            // "_context.Find passando o id.
            var obj = _context.Seller.Find(id);

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
            _context.SaveChanges();
        }

        // O que que vai ser atualizar um objeto do Tipo Seller?
        //
        // Update() corresponte a Ação de Edit (página) num CRUD.
        // Tanto que, depois de implementar este Método, eu tenho que verificar SE O LINK PRA 
        // AÇÃO DE Index (de Sellers) ESTÁ CORRETO.
        public void Update(Seller obj)
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
            //
            //if (_context.Seller.Any(x => x.Id == obj.Id) == false), foi substituído pelo abaixo, com 
            // "!" ANTES. Prá dizer que se NÃO (!) EXISTIR O ELEMENTO NO BD, COM O Id == AO Id DO obj.
            if (!_context.Seller.Any(x => x.Id == obj.Id))
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
                _context.SaveChanges(); // Para confirmar a Atualização (Update()).
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Então, se acontecer essa Exceção DbUpdateConcurrencyExceptiondo do
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
                // - Isso aqui é muito importante pra SEGREGAR AS CAMADAS:
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
                //  Ou seja, na nossa arquitetura geral, o controlador não conversa diretamente
                //  com os Repositories (acesso a Dados): Antes dos Repositories, vem os Services.
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
                //    Veja que na Arquitetura Geral, o Controlador não conversa (não manipula
                //    ou obtém dados) diretamente com um Repositorie (camada de acesso à dados):
                //
                //    Eu devo implementar:
                //    - SERVIÇOS para acessar/manipular os Dados das TABELAS do BD.
                //      Um exemplo é o SellerService, que obtém/manipula os dados da tabela
                //      Seller. 
                //
                //        Serviços (como exemplo o SellerService, DepartmentService) são para
                //        acessar as TABELAS (repositories) 'Seller' e 'Department'. 
                //        NÃO são para acessar as Classes (Entidades) Seller e Department. 
                //      
                //
                //    Foi o que fizemos:
                //     - Eu fiz 2 Serviços para acessar exceções do BD: estes serviços relançam
                //              as informações (exceções) do BD (Repositories).
                //     - Eu fiz 1 Serviço (SeedingService) pra popular o BD.
                //     - O FRAMEWORK tem 1 Serviço (services.AddDbContext<>) que nos <> eu informo
                //         qual será o meu Context pra montagem do BD.
                //     - Eu fiz 1 Serviço (DepartmentService), que depende (injeção de Dependência)
                //          do nosso Context (que usa Serviços do FRAMEWORK para uma sessão com o BD)
                //          para Listar os Departamentos.
                //     - Eu fiz este Serviço aqui (SellerService), que também depende do nosso Context
                //          para as operações relacionadas à Entidade Seller (Vendedor).
                //          Com as operações:
                //          - FindAll (para listar todos os Sellers),
                //          - FindById (para mostrar 1 Seller, baseado num Id),
                //          - Insert (Inserir 1 new Seller no BD).
                //          - Remove (Remover 1 Seller do BD).
                //          - Update (Atualiza os dados de 1 Vendedor).
                //      
                //      Assim, para um projeto bem feito (que segue uma arquitetura geral), um
                //      Controlador, por exemplo o SellersController, nunca terá acesso diretamente
                //      a um repositorie (CAMADA que acessa Dados).
                //      UM controlador deverá, através da CAMADA de Serviços, acessar a Camada de
                //      Acesso à Dados (repositorie).
                //        - Ele é uma Classe que herda da Controller (superClasse).
                //        - Ele é uma Classe então com suporte à View (página Razor).
                //        - Com ele eu não devo acessar ou manipular um Banco de Dados, DIRETAMENTE:
                //              Mas com os Serviços injetados nele (no controlador) para isso.
                //        - Assim, as Ações (requisições) do usuário, ele (controlador) vai usar os
                //          serviços injetados nele, e com o retorno desses serviços - como um
                //          controlador HERDA de uma Classe que tem suporte a View, ele também
                //          terá suporte a View - geralmente ele retornará uma View (pagina .cshtml)
                //          para o usuário com o mesmo nome da requisiçãoDoUsuárioNoNavegador
                //          (Ação), contendo o objeto retornado da operação (ação / método do
                //          controlador).               
            }
        }
    }
}
