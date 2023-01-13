using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Models; // Embora as 2 Classes relacionadas estejam  no mesmo namespace (....Data), a SalesWebMvc_NetContext está registrada em outro namespace (....Models).
using SalesWebMvc_.Net7.Models.Enums;

namespace SalesWebMvc_.Net7.Data
{
    public class SeedingService
    {
        // 1ª coisa a fazer: Registrar uma dependência desse SeedingService com o nosso DbContext:
        // O nosso DbContext é a Classe "SalesWebMvc_Net7Context". Ela já está nesta pasta "Data". Então, não preciso do import para ela.  
        // Importante: A pasta "Data" tem todas as nossas Classes relacionadas com o nosso Banco de Dados.
        private SalesWebMvc_Net7Context _context { get; set; }


        // O construtor recebe um objeto DbContext (SalesWebMvc_Net7Context) pra indicar que a dependência deve acontecer.
        //
        // Fizemos a injeção de dependência:
        //  Ou seja, quando um objeto SeedingService for criado, automaticamente ele vai receber uma instância (objeto que é a nossa BD) do Context também.   // O framework fará isso, porque nós registramos na Startup.cs (ConfigureServices() e Configure()) esta inclusão de mais um Serviço (o SeedingService) no SistemaDeInjeçãoDeDependênciaDoFramework.
        public SeedingService(SalesWebMvc_Net7Context context)
        {
            _context = context;
        }


        // Operação Seed(): vai ser responsável por popular a minha base de dados.
        public void Seed()
        {
            // Se já existe algum dado (registro) na minha base de dados, eu simplesmente não faço nada: Vou interroper a operação Seed, com o "return"
            // A Operação Any() do Linq, ela vai testar se existe algum dados nestas tabela abaixo:
            //    Any = Nenhum em português.
            if (_context.Department.Any() ||
                _context.Seller.Any() ||
                _context.SalesRecord.Any())
            {
                // So "return;", corta a execução deste Método.     // Isso pq ele não retorna nada (é void).
                return; // DB has been seeded.     // "O BD já foi populado"
            }

            // Passando pelo if, quer dizer que o DB não tem dados (registros nas tabelas). Aí nós vamos popular ele.   // Como nós vamos fazer isso? -Como estamos usando o workflow (fluxoDeTrabalho) CODE-FIRST (ou seja, eu crio os objetos, e daí eu crio as bases de dados), vamos fazer a instanciação dos objetos (registros) e mandar salvar este objetos no banco de dados.  
            Department d1 = new Department(1, "Computers");
            Department d2 = new Department(2, "Electronics");
            Department d3 = new Department(3, "Fashion");
            Department d4 = new Department(4, "Books");

            Seller s1 = new Seller(1, "Bob Brown", "bob@gmail.com", new DateTime(1998, 4, 21), 1000.0, d1);
            Seller s2 = new Seller(2, "Maria Green", "maria@gmail.com", new DateTime(1979, 12, 31), 3500.0, d2);
            Seller s3 = new Seller(3, "Alex Grey", "alex@gmail.com", new DateTime(1988, 1, 15), 2200.0, d1);
            Seller s4 = new Seller(4, "Martha Red", "martha@gmail.com", new DateTime(1993, 11, 30), 3000.0, d4);
            Seller s5 = new Seller(5, "Donald Blue", "donald@gmail.com", new DateTime(2000, 1, 9), 4000.0, d3);
            Seller s6 = new Seller(6, "Alex Pink", "bob@gmail.com", new DateTime(1997, 3, 4), 3000.0, d2);

            SalesRecord r1 = new SalesRecord(1, new DateTime(2018, 09, 25), 11000.0, SaleStatus.Billed, s1);
            SalesRecord r2 = new SalesRecord(2, new DateTime(2018, 09, 4), 7000.0, SaleStatus.Billed, s5);
            SalesRecord r3 = new SalesRecord(3, new DateTime(2018, 09, 13), 4000.0, SaleStatus.Canceled, s4);
            SalesRecord r4 = new SalesRecord(4, new DateTime(2018, 09, 1), 8000.0, SaleStatus.Billed, s1);
            SalesRecord r5 = new SalesRecord(5, new DateTime(2018, 09, 21), 3000.0, SaleStatus.Billed, s3);
            SalesRecord r6 = new SalesRecord(6, new DateTime(2018, 09, 15), 2000.0, SaleStatus.Billed, s1);
            SalesRecord r7 = new SalesRecord(7, new DateTime(2018, 09, 28), 13000.0, SaleStatus.Billed, s2);
            SalesRecord r8 = new SalesRecord(8, new DateTime(2018, 09, 11), 4000.0, SaleStatus.Billed, s4);
            SalesRecord r9 = new SalesRecord(9, new DateTime(2018, 09, 14), 11000.0, SaleStatus.Pending, s6);
            SalesRecord r10 = new SalesRecord(10, new DateTime(2018, 09, 7), 9000.0, SaleStatus.Billed, s6);
            SalesRecord r11 = new SalesRecord(11, new DateTime(2018, 09, 13), 6000.0, SaleStatus.Billed, s2);
            SalesRecord r12 = new SalesRecord(12, new DateTime(2018, 09, 25), 7000.0, SaleStatus.Pending, s3);
            SalesRecord r13 = new SalesRecord(13, new DateTime(2018, 09, 29), 10000.0, SaleStatus.Billed, s4);
            SalesRecord r14 = new SalesRecord(14, new DateTime(2018, 09, 4), 3000.0, SaleStatus.Billed, s5);
            SalesRecord r15 = new SalesRecord(15, new DateTime(2018, 09, 12), 4000.0, SaleStatus.Billed, s1);
            SalesRecord r16 = new SalesRecord(16, new DateTime(2018, 10, 5), 2000.0, SaleStatus.Billed, s4);
            SalesRecord r17 = new SalesRecord(17, new DateTime(2018, 10, 1), 12000.0, SaleStatus.Billed, s1);
            SalesRecord r18 = new SalesRecord(18, new DateTime(2018, 10, 24), 6000.0, SaleStatus.Billed, s3);
            SalesRecord r19 = new SalesRecord(19, new DateTime(2018, 10, 22), 8000.0, SaleStatus.Billed, s5);
            SalesRecord r20 = new SalesRecord(20, new DateTime(2018, 10, 15), 8000.0, SaleStatus.Billed, s6);
            SalesRecord r21 = new SalesRecord(21, new DateTime(2018, 10, 17), 9000.0, SaleStatus.Billed, s2);
            SalesRecord r22 = new SalesRecord(22, new DateTime(2018, 10, 24), 4000.0, SaleStatus.Billed, s4);
            SalesRecord r23 = new SalesRecord(23, new DateTime(2018, 10, 19), 11000.0, SaleStatus.Canceled, s2);
            SalesRecord r24 = new SalesRecord(24, new DateTime(2018, 10, 12), 8000.0, SaleStatus.Billed, s5);
            SalesRecord r25 = new SalesRecord(25, new DateTime(2018, 10, 31), 7000.0, SaleStatus.Billed, s3);
            SalesRecord r26 = new SalesRecord(26, new DateTime(2018, 10, 6), 5000.0, SaleStatus.Billed, s4);
            SalesRecord r27 = new SalesRecord(27, new DateTime(2018, 10, 13), 9000.0, SaleStatus.Pending, s1);
            SalesRecord r28 = new SalesRecord(28, new DateTime(2018, 10, 7), 4000.0, SaleStatus.Billed, s3);
            SalesRecord r29 = new SalesRecord(29, new DateTime(2018, 10, 23), 12000.0, SaleStatus.Billed, s5);
            SalesRecord r30 = new SalesRecord(30, new DateTime(2018, 10, 12), 5000.0, SaleStatus.Billed, s2);

            // Agora vamos mandar adicionar nossos objetos no Banco de Dados.
            // Como a gente faze isso usando o Entity Framework?.
            //    Usando o Método AddRange().
            //     Ele permite que eu adicione VÁRIOS objetos de uma só vez.
            _context.Department.AddRange(d1, d2, d3, d4);

            _context.Seller.AddRange(s1, s2, s3, s4, s5, s6);

            _context.SalesRecord.AddRange(
                r1, r2, r3, r4, r5, r6, r7, r8, r9, r10,
                r11, r12, r13, r14, r15, r16, r17, r18, r19, r20,
                r21, r22, r23, r24, r25, r26, r27, r28, r29, r30
            );


            // Para efetivar estas alterações no BD, eu tenho que chamar o
            // _context.SaveChanges(). 
            //     Ele Salva e Confirma as Alterações no Banco de Dados.
            _context.SaveChanges();

        }
    }
}
