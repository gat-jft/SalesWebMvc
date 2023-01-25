using System; // Para o DateTime, cw etc.
using System.Collections.Generic; // Para o Tipo ICollection<T> (interface), que tanto pode ser uma Lista como um 
using System.Linq; // Para operações em Coleções (Lista, BD, HasSet, Arquivo etc) usando o Linq. .


namespace SalesWebMvc_.Net7.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime BirthDate { get; set; } // DataDeNascimento
        public double BaseSalary { get; set; } // Salário

        // Associação 1 prá 1: Vendedor "TEM 1" (é composto por 1) Departamento. 
        public Department Department { get; set; }

        // Com esta declaração (colocando o sufixo "Id" depois da Classe (Department) tem tem o relacionamento "PARA 1" com esta Classse Selle)), o framework já sabe que este campo é um INT (correspondente ao Id do Department) e que NÃO pode ser NULO 
        // Com isso aqui, a gente tá avisando pro FRAMEWORK que este Id vai ter que existir. Uma vez que o tipo int (INT) não pode ser NULO.
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
