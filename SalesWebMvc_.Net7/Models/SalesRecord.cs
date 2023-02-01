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

        // Construtor Default é OBRIGATÓRIO, se eu colocar o Construtor com argumentos. O Framework precisa dele assim.
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
