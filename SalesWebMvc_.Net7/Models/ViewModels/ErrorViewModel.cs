namespace SalesWebMvc_.Net7.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }        // Os tipos comuns de valor não podem ter um valor null. No entanto, você pode criar tipos de valor anulável acrescentando uma ? após o tipo. Por exemplo, int? é um tipo int que também pode ter o valor null. 

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}