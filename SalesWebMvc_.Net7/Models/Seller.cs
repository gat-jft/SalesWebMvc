using System; // Para o DateTime, cw etc.
using System.Collections.Generic; // Para o Tipo ICollection<T> (interface), que tanto pode ser uma Lista como um 
using System.ComponentModel.DataAnnotations; // Para usar o ANNOTATION [Display]
using System.Linq; // Para operações em Coleções (Lista, BD, HasSet, Arquivo etc) usando o Linq. .
using System.Net;


namespace SalesWebMvc_.Net7.Models
{
    public class Seller
    {
        // Classe (Entidade do meu negocio), que vai conter dos dados para uma View, GERALMENTE a Tela Index.
        //
        // Mas eu posso criar uma outra View além da Index, que pode usar um Seller, no Controlador "Sellers":
        //    Neste caso, no arquivo da View eu coloco no @model dele o caminho.Seller.


        public int Id { get; set; }

        
        //[Required]     // ANNOTATION prá dizer que este campo (Name) é OBRIGATÓRIO (seu preenchimento).
        //[Required(ErrorMessage = "Name required")] // Eu posso colocar uma mensagem (de erro) customizada, para caso o usuário não preencher este Atributo. 
        [Required(ErrorMessage = "{0} required")] // Substituindo os 2 acima, na {0} é prá string colocar o Nome do Atributo.
        //[StringLength(60, MinimumLength = 3, ErrorMessage = "Name size between 3 and 60")] // [StringLength(60, ...), é pra eu estabelecer limites de Mínimo e Máximo pro tamanho deste Nome.     // 60 é o Máximo (de caracteres claro!), 3 é o Mínimo)   // E eu posso colocar uma mensagem de erro personalizada, no 3° argumento da ANNOTATION (Classe). Se eu não colocar uma mensagem de erro personalizada (como esta aqui: "o tamanho do Nome deve ser entre 3 e 60), o FRAMEWORK ele tem uma mensagem padrão. 
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size between {2} and {1}")] // Eu posso PARAMETRIZAR a string (" ") mensagem de erro personalizada "o tamanho do Nome deve ser entre 3 e 60".    // Isto é, eu posso pegar os valores aqui, que eu defini antes (Name, 60, 3).    // Para que a string mostre, eu vou colocar {0}, {1} e {2} na string " eu colocar na string "Name size between 3 and 60".    // Assim: "{0} size between {2} and {1]".     // O FRAMEWORK já sabe que {0} é o Nome do Atributo, {2} é o 2° parâmetro que eu coloquei no parâmetro () do Construtor deste [StringLength()] aqui. {1} é o 1° parâmetro que eu coloquei no Construtor deste [StringLength()].
        public string Name { get; set; }


        [EmailAddress(ErrorMessage = "Enter a valid email")] // Quando o usuário colocar um Email, cujo formato não seja de email, como sem o @.
        [Required(ErrorMessage = "{0} required")] // Substituindo os 2 acima, na {0} é prá string colocar o Nome do Atributo.
        [DataType(DataType.EmailAddress)]  // Transforma o email de um texto plano para um LinkDeEmail (com o sublinhado em baixo do texto).
        public string Email { get; set; }
               
        
        // ANNOTATION [Display] serve para o que que eu quero que aparece de rótulo lá nas minhas telas.
        // 
        // Então, se eu não colocar [Display] aqui, o framework irá colocar o nome do Atributo.
        //
        // Mas, colocando o ANNOTATION [Display()], o Name = será como todas as minhas telas irá 
        // mostrar. No caso, em qualquer View que for exibir este Atributo, o rótulo (etiqueta)
        // terá "Birth Date" (com ESPAÇO), e não "BirthDate".
        [Required(ErrorMessage = "{0} required")] // Substituindo os 2 acima, na {0} é prá string colocar o Nome do Atributo.
        [Display(Name = "Birth Date")]          // Para mostrar o rótulo (Label) "Birth Date" (separado), e não "BirthDate" 
        [DataType(DataType.Date)]               // Mostra o Atributo BirthDate sem a HH:MM.    // Mas como o locale é dos Estados Unidos, ainda continha mostrando MM/dd/yyyy. Para inverter, tenho que usar o [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}" )] ABAIXO.      // DataType é o TipoDoDado, não TipoDeData. NÃO CONFUNDIR ENTÃO.    // No caso. o tipoDoDado é um Tipo Data.
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy}" )] // Para exibir a Data como "dd/MM/YYYY".    // 0 (zero) indica o VALOR do Atributo, e dd/MM/yyyy a formatação.     // Invertido do que geralmente é mostrado ("MM/dd/yyyy'), por causa do locale do Estados Unidos.
        public DateTime BirthDate { get; set; } // DataDeNascimento


        [Required(ErrorMessage = "{0} required")] // Validação de Requerido.
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")] // Validação de faixa de valores para números.    // O Salário tem que ser de no mínimo 100 e no máximo 50000. E a mensagem de erro tá falando: "o {Base Salary} deve ser de {100} até {500000}". Claro que as {} não aparecem.  
        [Display(Name = "Base Salary")]   // Customizamos para que o Nome deste Dado quando for aparecer num no rótulo (label), será exibido "Base Salary" (separado) e não + "BaseSalary". 
        [DisplayFormat(DataFormatString = "{0:F2}")]  // Para formatar um número com 2 casas decimais. Nas {}, 0 (Zero) indica o valor do Atributo. E este valor vai ter a formatação F2.
        public double BaseSalary { get; set; } // Salário


        //Não precisa de eu colocar o [Required] aqui no Department, porque como a gente colocou
        // o MACETE do int
        //
        // Associação 1 prá 1: Vendedor "TEM 1" (é composto por 1) Departamento. 
        //
        // Este atributo estava como obrigatório (sem o ? no final do Tipo). Daí, estava dando erro de Validação na View Create: quando eu clicava no botão pra Criar um Novo Seller (Vendedor), o FRAMEWORK lançava a exceção de que este campo é obrigatório.  // Então, baseado no teste ((if (!ModelState.IsValid) { ... }) de validação do do Model (Entidade), o eu deixo uma Property (campo / atributo) sem o '?', o próprio FRAMEWORK reclama no formulário da View, com a mensagem de erro: "The Field is required". 
        public Department? Department { get; set; }

        // Adicionei este campo, para que não exista no BD, um Vendedor sem um DepartmentId (campo Id do Department).
        // Com esta declaração (colocando o sufixo "Id" depois da Classe (Department) tem tem o relacionamento "PARA 1" com esta Classse Seller)), o framework já sabe que este campo é um INT (correspondente ao Id do Department) e que NÃO pode ser NULO 
        // Com essa Property declarada assim "DepartmentId" (ou seja, é o mesmo nome da Classe Department com o sufixo Id),
        // o FRAMEWORK é capaz de detectar que eu quero guardar na Classe (nesta), o Id do Departamento, e aí se eu colocar
        // esta Propriedade como int, o int que é um struct não pode ser NULO, e aí o FRAMEWORK vai garantir
        // vai garantir que a Base de Dados vai ser criada corretamente.
        // 
        // Com isso então, a gente tá avisando pro ENTITY FRAMEWORK prá ele garantir que este Id (Id do Departamento),
        // Uma vez que o Tipo int não pode ser NULO.
        //
        // Então, com este int aqui, ele vai ser NATURALMENTE obrigatório
        // (o seu preenchimento, claro!), antes de enviar o Seller para
        // o Banco de Dados. Dispensando assim a ANNOTATION [Required] neste Campo (esta Property).
        //
        // E também a gente escolhe ele pela Caixinha de Seleção né?.
        //
        // Se este Atributo pudesse ser nulo, estaria declarado com (int?) 
        // assim: 
        // public int? DepartmentId { get; set; }
        public int DepartmentId { get; set; }


        // Associação ParaMuitos, do Vendedor com (MUITAS) as Vendas.      // 1 Vendedor "TEM MUITAS" Vendas.       // É uma composição: 1 Vendedor é composto (tem) Vendas (SalesRecord).      // Usamos o Tipo ICollection<T> porque ele serve para qq coleção (List<T>, HasSet<T etc).       // E já o instanciamos: no caso, uma Lista<T> vazia.  
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        // Construtor vazio (default).    // Se eu não tivesse o Construtor com argumentos, eu não precisaria deste aqui.
        public Seller()
        {
        }

        // Construtor com argumentos.     // Não incluir os atributos que forem Coleções, no caso aqui o Sales.
        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        // Adicionar 1 Venda para o Seller (Vendedor). Na nossa Lista<Venda> claro!.
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        // Remover 1 Venda do Vendedor (da nossa Lista<Venda>)
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        // Função para calcular o Total de Vendas, da nossa Lista<Venda>.
        public double TotalSales(DateTime initial, DateTime final)
        {
            // Retorne a Coleção Sales, com o filtro (Where) + a soma.  // Sales é a Lista de Vendas, associada com este Vendedor (Classe Seller).    // sr é todo objeto SalesRecord (RegistroDeVendas).      // Filtrar (Where) todos da Lista "Sales" (Vendas), ou seja filtrar todas as vendas cuja Date seja >= initial e <= final. Depois somar os valores (Amount) dessas vendas filtradas.
            // Estou usando o Linq, porque temos operações em uma Coleção.
            //
            // Eu vou pegar todo objeto sr nesta Lista (Sales). E, depois aplicar a soma das vendas (Amount) cada objeto sr da Lista filtrada.
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
                     

            // SOMENTE COM 1 LINHA DE CÓDIGO ACIMA, NÓS RESOLVEMOS O PROBLEMA DE CALCULAR O TOTAL DE VENDAS DE 1 VENDEDOR NO INTERVALO DE DATAS.
            //
            // USEI AS FUNÇÕES DO Linq. E COM ISSO, ELIMINEI AS VÁRIAS LINHAS DE CÓDIGO.  
            //
            //double totalSales = 0;
            //foreach (var item in Sales)
            //{
            //    if (item.Date >= initial && item.Date <= final)
            //    {
            //        totalSales += item.Amount;
            //    }                
            //}
            //return totalSales;
        }
    }
}
