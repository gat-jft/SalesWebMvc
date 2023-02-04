using System;

namespace SalesWebMvc_.Net7.Services.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        // Esta Classe (NotFoundException = ExceçãoDeNãoEncontrado) ela vai HERDAR (:)
        // da Classe ApplicationException.
        //
        // Construtor dela:
        // Esta classe vai receber um "string message", e simplesmente ela vai repassar esta 
        // "message" lá pra Classe base.
        public NotFoundException(string message) : base(message) 
        { 
        }
    }
}
