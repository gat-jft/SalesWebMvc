using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Data;

// Embora esta Classe esteja na pasta Data, ela está registrada no namespace SalesWebMc_7.Models
namespace SalesWebMvc_.Net7.Models
{
    public class SalesWebMvc_Net7Context : DbContext 
    {
        public SalesWebMvc_Net7Context (DbContextOptions<SalesWebMvc_Net7Context> options)
            : base(options)
        {
        }

        // SalesWebMvc_Net7Context (Classe) é um DbContext (realiza uma sessão com o nosso Banco de Dados. É a Classe que realiza esta sessão).


        // Property´s tipo DbSet<T>. Representam as tabelas no meu Db (DbContext). 
        //
        // Podemos apagar o caminho "SalesWebMvc_.Net7.Models" de dentro dos <>, uma vez que ele é desnecessário. Porque já tem namespace no import ("using") na linha 6.     // Note que ele já está ESMAECIDO (desnecessário).
        public DbSet<SalesWebMvc_.Net7.Models.Department> Department { get; set; } = default!; // "= default!;" também é DESNECESSÁRIO, para eu declarar os Atributos (Property´s). 
        public DbSet<SalesRecord> SalesRecord { get; set; } = default!;
        public DbSet<Seller> Seller { get; set; } = default!;
    }
}
