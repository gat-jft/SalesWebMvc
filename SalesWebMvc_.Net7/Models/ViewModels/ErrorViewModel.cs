namespace SalesWebMvc_.Net7.Models.ViewModels
{
    public class ErrorViewModel
    {
        // "string?" ou "qualquerTipo?", quer dizer que é OPCIONAL:
        // Eu posso ter esta string ou NÃO (nulo).
        // 
        // Este dado (RequestId), é um Id interno aqui da requisição,
        // que a gente pode também mostrar nessa Página de Erro.
        public string? RequestId { get; set; }        

        // Acrescentei esta Property, Só pra gente ter condição de
        // também de acrescentar uma MENSAGEM CUSTOMIZADA nesse
        // objeto.
        public string Message { get; set; }







        // Esta declaração não é uma Property.
        // Escrita dessa forma, isso é um FUNÇÃO:
        // Igual à uma EXPRESSÃO LAMBDA do Linq.
        //   
        // ShowRequestId é uma funçãoZinha, prá testar se o Id existe.
        // A função tem um MACETINHO:
        // A função vai retornar (=>), se ele NÃO (!) É nulo ou vazio.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}