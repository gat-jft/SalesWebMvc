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
            _context.Add(obj);
            _context.SaveChanges();

            // Só adicionar o obj Seller no BD não vai adicionar.
            // Eu preciso também confirmar.
            // É o _context.SaveChanges().

        }
    }
}
