using SalesWebMvc_.Net7.Models; // Para ele enxergar o NOSSO DbContext, que é o SalesWebMvc_Net7Context.     // Embora ele esteja na pasta Data, ele foi declarado com o namespace para a pasta .Models (SalesWebMvc_Net7.Models).


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
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
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
    }
}
