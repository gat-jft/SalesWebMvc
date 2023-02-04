using System;

namespace SalesWebMvc_.Net7.Services.Exceptions
{
    public class DBConcurrencyException : ApplicationException
    {
        // Esta Classe (DBConcurrencyException = ExceçãoDeSimultaneidadeDoBD) ela vai HERDAR (:)
        // da Classe ApplicationException.
        //
        // Construtor dela:
        // Esta classe vai receber um "string message", e simplesmente ela vai repassar esta 
        // "message" lá pra Classe base.
        public DBConcurrencyException(string message) : base(message)
        { 
        }
    }
}
