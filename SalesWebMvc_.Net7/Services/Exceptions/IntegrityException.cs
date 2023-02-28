using System;

namespace SalesWebMvc_.Net7.Services.Exceptions
{
    // Esta é a nossa Exceção Personalizada de Serviço (pasta Services), para
    // ERROS de Integridade Referencial.
    //    
    // É para o caso de Deleção de 1 Vendedor do BD por exemplo,
    // para que não uma Venda não fique sem 1 Vendedor.
    // Isso é Integridade Referencial:
    //    O bd não deixa apagar 1 registro, que tenha 1 referência para outra 
    //    registro em outra Tabela.
    //        
    //    Venda (SalesRecord) tem uma referência (ponteiro) para 1 Vendedor (Seller).
    //    Ela não tem o Vendedor, mas tem o endereço (referência) de onde
    //    está 1 Vendedor na memória.
    public class IntegrityException : ApplicationException
    {
        // Construtor básico aqui pra esta nossa Exceção Personalizada:
        // É o public IntegrityException, RECEBENDO um string message;
        //   E eu vou simplesmente repassar esta chamada (método Construtor)
        //   lá pra super-Classe: É o “: base(message)”.
        public IntegrityException(string message) : base(message)
        {
        }
    }
}
