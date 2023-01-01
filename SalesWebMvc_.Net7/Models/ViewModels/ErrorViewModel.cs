namespace SalesWebMvc_.Net7.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }        // Os tipos comuns de valor n�o podem ter um valor null. No entanto, voc� pode criar tipos de valor anul�vel acrescentando uma ? ap�s o tipo. Por exemplo, int? � um tipo int que tamb�m pode ter o valor null. 

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}