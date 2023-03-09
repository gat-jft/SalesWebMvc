using Microsoft.EntityFrameworkCore; // Para a função Include
using SalesWebMvc_.Net7.Models;
using System.Data.Common;
using System.Drawing;
using System.Text;

namespace SalesWebMvc_.Net7.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvc_Net7Context _context;

        public SalesRecordService(SalesWebMvc_Net7Context context)
        {
            _context = context;
        }

        // Prá ficar mais bacana, este método FindByDate, passou a ser uma operaão ASSÍNCRONA.
        // Como?
        // - No return, ao invés de ser só a Lista, vai ser um Task dessa Lista:
        //     Task<List<SalesRecord>>
        // - Decorar com a palavrinha “async” no comecinho da Declaração do Método.
        // - Trocar o Nome do Método, colocando nele o sufixo "Async". Não é obrigatório, mas é uma boa prática.   // Ficou então, FindByDateAsync.
        // - Trocar o ToList(), pelo ToListAsync(),
        // - Colocar a palavrinha “await” no comecinho, da operação que tem
        //   a chamada assíncrona (o ToListAsync()). 
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {

            // IMPORTANTE:
            // SEMPRE QUE EU QUISER USAR (consutas) O BD, IMPORTO O NAMESPACE System.Linq;

            // "var" está inferindo o IQueryable.
            // IQueryable é uma Interface do Linq.
            // Fornece funcionalidades para avaliar consultas em uma
            // fonte de dados em que o tipo de dados NÃO foi
            // especificado.
            //
            // Se o tipo é conhecido, eu já posso instanciar um objeto
            // Queryable<T>, para fazer a Consulta ao BD.
            // - Então, no exemplo, "result" é do Tipo (inferido pelo compilador) IQueryable.
            //   INFERIDO por causa da análise do que a variável recebe, baseado na consulta em
            //   SQL: "from obj in ...".
            // 
            // O importante eu saber é que:
            // SEMPRE QUE EU QUISER USAR (consultas) O BD, IMPORTO O NAMESPACE System.Linq;
            // - O Linq é o namespace que oferece Classes e Interfaces compatíveis com CONSULTAS à BD.
            //     Estas suas Classes usam usam LINQ (Consulta Integrada à Linguagem).
            // - As Classes e Interfaces do Linq são: Enumerable, Queryable, IEnumerable<TElement>,
            //   IQueriable etc.
            //     Exemplos:
            //       IEnumerable<TElement> e IOrdered, representam o resultado de uma operação de
            //       classificação. 
            // - Eu posso criar um objeto declarando o tipo. Ou criando um tipo var, que o compilador
            //   já infere o Tipo (Classe) deste objeto.            //   
            // - Criados os objetos (variáveis) de uma Classe do Linq, eu chamo neles métodos para minhas
            //   Consultas
            //   Exemplo: 
            //      Criei a variável "result" com o tipo var.  
            //            // É bom sempre usar o var na Declaração da variável, por praticidade e para que,
            //            // baseado no (Método do Linqu OU instrução SQL) que eu colocar na
            //            // atribuição desta variável, o compilador inferirá o Tipo.
            //       
            //      Daí faço chamadas de métodos dos métodos principais de Consultas:
            //      Where ou Select.
            //        - Where eu vou filtrar os dados.
            //        - Select eu vou alterar a forma como os (registros) dados serão apresentados
            //     Após do Select ou Where, eu posso usar outros métodos:
            //        Sum, Avg, OrderBy, Min, Max, FirstOrDefault etc. 
            //   
            //  Baseado na chamada (em SQL) "from obj in _context.SalesRecord select obj;", o compilador
            //  sabe que o tipo para o var será IQueryable.
            //  - Daí, da variável "result" terá a TABELA 'SalesRecord'.
            //    Da variável (que agora representa a TABELA 'SalesRecord', eu posso chamar os métodos
            //    da Classe IQueryable, que são relacionados à CONSULTA DE BD:
            //    
            //  Exemplos
            //    result.ToList() // converte a tabela 'SalesRecord' para Lista.
            //    result.orderByDescending() // ordena a tabela 'SalesRecord' decrescente. 
            //    result.
            //
            //  E ainda posso faze a JUNÇÃO de tabelas.
            //  // SÓ QUE PARA USAR O MÉTODO (Include) QUE FAZ JUNÇÃO, EU PRECISO DO NAMESPACE
            //  // Microsoft.EntityFrameworkCore.
            //     result.Include(x => x.Seller) //Faz a JOIN da Tabela 'SalesRecord' (variável result), com tabela (Seller).
            //           .Include(x => x.Seller.Department) // Depois, faz a JOIN com os Departamentos do Vendedor.
            //
            // Mais em: 
            //
            // result = consulta. // result RECEBE a tabela SalesRecord.
            var result = from obj in _context.SalesRecord select obj;
           
            // Se eu informei a DataMínima (minDate.HasValue)
            if (minDate.HasValue)
            {
                // result é o objeto (do Tipo IQueryable), que vai avaliar a consulta
                result = result.Where(x => x.Date >= minDate.Value);
            }

            // Da mesma forma, eu vou fazer a DataMáxima:
            // Se foi informado uma DataMáxima (maxDate.HasValue)?
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            // Prá executar minha consulta, uso o ToList().
            // Porque?
            // - Porque as operações do Linq não executam a consulta. Elas
            //   só preparam a consulta. :
            //return result.ToList();
            //
            // Só que eu vou acrescentar mais coisas aqui no Método:
            // - Eu vou fazer um JOIN com a tabela de Vendedor, e com a
            //   tabela de Departamento.
            // - E depois, eu ainda vou ORDENAR DECRESCENTEMENTE por Data.
            //
            // TUDO ISSO EU VOU FAZER COM O Linq E COM AQUELA FUNÇÃO Include() DO ENTITY FRAMEWORK.
            return await result
                   .Include(x => x.Seller) // Isso aqui faz o JOIN das Tabelas prá mim.    // Include é do Linq (namespace Linq).
                   .Include(x => x.Seller.Department) // Assim também eu faço o JOIN com a Tabela de Departamentos.
                   .OrderByDescending(x => x.Date)  // Agora prá terminar, eu vou fazer o OrdeByDescending(), do Linq.   // Nos "()" eu vou informar que eu vou ORDENAR por Data (x => x.Date).
                   .ToListAsync();
        }

        // O Tipo de retorno, vai ser uma Task de List de Igrouping de <Department, SalesRecodr>. 
        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingsync(DateTime? minDate, DateTime? maxDate)
        {
            
   


            // Estamos preparando o objeo do Tipo IQueryable.  // result = consulta. 
            var result = from obj in _context.SalesRecord select obj;

            // Agora vamos colocar as restrições de data.
            //
            // Se eu informei a DataMínima (minDate.HasValue)
            if (minDate.HasValue)
            {
                // result é o objeto (do Tipo IQueryable), que vai avaliar a consulta
                result = result.Where(x => x.Date >= minDate.Value);
            }

            // Da mesma forma, eu vou fazer a DataMáxima:
            // Se foi informado uma DataMáxima (maxDate.HasValue)?
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            // Prá executar minha consulta, uso o ToList().
            // Porque?
            // - Porque as operações do Linq não executam a consulta. Elas
            //   só preparam a consulta. :
            //return result.ToList();
            //
            // Só que eu vou acrescentar mais coisas aqui no Método:
            // - Eu vou fazer um JOIN com a tabela de Vendedor, e com a
            //   tabela de Departamento.
            // - E depois, eu ainda vou ORDENAR DECRESCENTEMENTE por Data.
            //
            // TUDO ISSO EU VOU FAZER COM O Linq E COM AQUELA FUNÇÃO Include() DO ENTITY FRAMEWORK.
            return await result
                   .Include(x => x.Seller) // Isso aqui faz o JOIN das Tabelas prá mim.    // Include é do Linq (namespace Linq).
                   .Include(x => x.Seller.Department) // Assim também eu faço o JOIN com a Tabela de Departamentos.
                   .OrderByDescending(x => x.Date)
                   .GroupBy(x => x.Seller.Department)  // Agora prá terminar, eu vou fazer o OrdeByDescending(), do Linq.   // Nos "()" eu vou informar que eu vou ORDENAR por Data (x => x.Date).
                   .ToListAsync();
        }
    }   
}
