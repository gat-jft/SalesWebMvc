namespace SalesWebMvc_.Net7.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string? Name { get; set; } // ? é para a Propriedade ser também anulável (receber o valor null).
    }
}
