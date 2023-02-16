using System; // Import necessário para o Tipo DateTime, cw etc.
using SalesWebMvc_.Net7.Models.Enums; // para ele reconhecer o tipo enumerado SaleStatus.

namespace SalesWebMvc_.Net7.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; } // Quantia
        public SaleStatus Status { get; set; }

        // Associação "1 pra 1": Cada SalesRecord (Venda) possui 1 Vendedor.
        public Seller Seller { get; set; }

        // Construtor Default é OBRIGATÓRIO, se eu colocar o Construtor com argumentos.
        // O Framework precisa dele assim.
        //
        // Acho que é pelo fato de, quando eu crio uma Migration (estrutura do BD), o FRAMEWORK
        // criará um novo Banco de Dados no MySql Workbench. Só que com as tabelas vazias.
        //
        // Com isso, o FRAMEWORK me dá a opção de CODE FIRST.
        //
        // Então, quando eu crio uma nova Migration, o BD é criado, mas vazio.
        // 
        // Aí, SE EU COLOCO CONSTRUTOR SÓ COM ARGUMENTO, O FRAMEWORK NÃO TERÁ CONDIÇÃO NA
        // MIGRATION, PORQUE NO CASO PRECISA DE DADOS.
        // Aí, a aplicação poderá ser quebrada (ERRO).
        //        
        // // É possível então, com a criação das Migration com umscript para inserir dados no
        // // BD. Mas é bem mais difícil, e assim eu não estaria usando a estratégia do
        // // do CODE FIRST. 
        // //
        // // Melhor usar então, o Construtor () e o com argumentos, para fazer parte da
        // // estratégia do CODE FIRST:
        // // - Crio 1 Construtor vazio e 1 com argumentos.
        // // - Qualquer alteração na estrutura do BD, se o BD já existir, eu vou no MySql
        // //   e deleto (DROPO) este Banco de Dados.
        // // - Eu já devo ter um Serviço que povoará meu BD, registrado no sistema de injeção
        // //   de dependência (um AddScoped<Serviço>;), no método ConfigureServices, lá na
        // //   Startup.cs. Além de registrar este Serviço de povoar dados no BD, ele deve
        // //   ser colocado no método parâmetro Configure() lá da Startup.cs, para que
        // //   toda vez que eu rodar (executar) a aplicação, uma instância deste Serviço seja
        // //   resolvida AUTOMATICAMENTE, povoando meu BD SE NÃO TIVER DADOS NELE. 
        // // - Adiciono a nova Migration, que criará o BD vazio.
        // // - Rodo a aplicação. Como o Serviço de povoar o BD está nos argumentos () do método
        // //   Configure, automaticamente o framework coloca este serviço à minha disposição,
        // //   e povoará o BD, porque ele está vazio. E este Serviço para povoar o meu 
        // //   BD, o framework irá colocar a minha diposição. Ele verá que o BD está vazio,
        // //   então povoará este BD.
        // //
        // // Se eu não coloco um Construtor vazio, como o framework não terá condição de criar
        // // um BD vazio. PORQUE EU DEFINI QUE OS EU SÓ POSSO TER OBJETOS COM DADOS.
        // //
        // //   Se o BD já existir, eu devo o FRAMEWORK criará um BD vazio
        // //   Lembrando que se não colocar um Construtor com argumentos, eu não preciso do
        // //   Construtor vazio.
        // //
        // // CODE FIRST = Como o próprio nome já diz, ele se refere à estratégia de criar o
        // // código da aplicação primeiro, ou seja, as famosas classes da orientação a objetos
        // // para, a partir disso, criar a nossa base de dados, com toda a sua estrutura.
        // // No artigo anterior realizamos exatamente a operação inversa, criando primeiramente
        // // o banco de dados e depois as nossas classes.
        // // https://medium.com/@alexandre.malavasi/s%C3%A9rie-entity-framework-code-first-parte-3-6d807b4addbf
        public SalesRecord()
        {
        }

        // Construtor com argumentos também é OBRIGATÓRIO.     // Quando o parâmetro for uma Lista, não informar.
        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
