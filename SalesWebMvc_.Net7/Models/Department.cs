using System; // Para ter o DateTime, cw etc.
using System.Collections.Generic; // Para reconhecer o tipo ICollection (Interface) que pode ser tanto uma lista, como 
using System.Linq; // Para expressões lambda, que irão para o DbSet e depois para o MySql.


namespace SalesWebMvc_.Net7.Models
{
    public class Department
    {
        // Classe (Entidade do meu negocio), que vai conter dos dados para uma View, GERALMENTE a Tela Index.
        //
        // Mas eu posso criar uma outra View além da Index, que pode usar um Department, no Controlador "Departments":
        //    Neste caso, no arquivo da View eu coloco no @model dele o caminho.Department.
        
        public int Id { get; set; }
        public string? Name { get; set; } // ? é para a Propriedade ser também anulável (receber o valor null).

        // Associação ParaMuitos: no caso 1 Department "tem muitos" Vendedores.      // É uma composição: 1 Departamento é composto por vários Vendedores.     // Usamos o Tipo ICollection<T> porque ele serve para qq coleção (List<T>, HasSet<T etc).   // E já o instanciamos: no caso, uma Lista<T> vazia.  
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>(); // Esta Property chama-se "Sellers" no PLURAL, porque são muitos vendedores: uma Lista.

        public Department()
        {
        }

        // Construtor com argumentos.     // Não incluir os atributos que forem Coleções, no caso o Sellers.
        public Department(int id, string? name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        // Operação para calcular o TotalDeVendas() no Departamento, num intervalo de Datas.
        public double TotalSales(DateTime initial, DateTime final)
        {
            // Vou fazer o "return", no caso a listinha "Sellers" prá pegar a listinha de vendedores desse Departamento. E aí, eu vou chamar a Função Soma, passsando para esta Soma uma expressão lambda que filtra apenas as Vendas nesse período de Data. 
            //
            // A expressão lambda (seller => seller.TotalSales(initial, final) é o FILTRO.


            // Na expressão lambda (função anônima), "seller" é a referência a um objeto Vendedor (no caso a Classe Seller), e o retorno é esta referência.MétodoDestaReferência.
            //
            // Eu estou pegando cada vendedor "seller" da minha Lista, chamando o TotalSales() do vendedor (seller) naquele período inicial e final. E aí, eu faço uma Soma (Sum) prá todos dos Vendedores (Sellers) do meu Departamento.
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
