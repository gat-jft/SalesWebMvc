namespace SalesWebMvc_.Net7.Models.ViewModels
{
    public class ErrorViewModel
    {
        // "string?" ou "qualquerTipo?", quer dizer que � OPCIONAL:
        // Eu posso ter esta string ou N�O (nulo).
        // 
        // Este dado (RequestId), � um Id interno aqui da requisi��o,
        // que a gente pode tamb�m mostrar nessa P�gina de Erro.
        public string? RequestId { get; set; }        

        // Acrescentei esta Property, S� pra gente ter condi��o de
        // tamb�m de acrescentar uma MENSAGEM CUSTOMIZADA nesse
        // objeto.
        public string Message { get; set; }







        // Esta declara��o n�o � uma Property.
        // Escrita dessa forma, isso � um FUN��O:
        // Igual � uma EXPRESS�O LAMBDA do Linq.
        //   
        // ShowRequestId � uma fun��oZinha, pr� testar se o Id existe.
        // A fun��o tem um MACETINHO:
        // A fun��o vai retornar (=>), se ele N�O (!) � nulo ou vazio.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}