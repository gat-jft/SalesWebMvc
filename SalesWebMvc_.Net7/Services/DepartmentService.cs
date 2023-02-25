using SalesWebMvc_.Net7.Models; // É onode está a nossa Classe"SalesWebMvc_Net7Context".    // Context é o nosso contexto de BD, no C#.
using System.Linq; // Porque vamos precisar do Linq: vamos usar o Função (Método) OrderBy do Linq. 
using System.Collections.Generic; // Para importar a Lista (Classe List).
using System.Threading.Tasks; // Para eu poder criar as minhas operações (Métodos) assíncronas. Ou seja, funções que retornam o Tipo Task (Task<tipo retornado>), decoradas com a palavrinha "async", com o SUFIXO "Async" no Nome (nos nomes das Ações eu não coloco este SUFIXO), e com a "await" (aguardando pela resposta do chamada assíncrona). 
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore; // Para operações que são do Microsoft.EntityFrameworkCore. ToList() é do Linq. Operações do Linq (Sum,  OrderBy, Select etc), são operações síncronas.      // Já o ToListAsync é uma outra versão do ToList(). E não é do Linq. É do Microsoft.EntityFrameworkCore. E é uma operação ASSÍNCRONA.
using System.Collections.Immutable;

namespace SalesWebMvc_.Net7.Services
{
    public class DepartmentService
    {
        // Este serviço (classe) é para gerar uma Listinha de Departamentos.
        // Para que ele possa funcionar, ele vai depender (injeção de dependênci) do nosso Context (SalesWebMvc_Net7Context)
        // do FRAMEWORK. Ou seja, ele depende do BD.
        //
        // Esta Classe DepartmentService, terá com este Atributo, a dependência com o Context do Entity Framework.
        //
        // Um Atributo declarado com o "readonly" (acesso apenas leitura), não pode ser declarado como PROPERTY.
        // - Quando eu digitei o snipptet "prop" (para criar uma Property), deu mensagem de ERRO do compilador.
        // - Ficou assim, como se fosse Property.
        //       private readonly SalesWebMvc_Net7Context _context { get; set; }
        // - Como eu coloquei com o "readonly" (como a maioria dos programadores da comunidade), é claro que ele não ia aceita o "{ get; set; }" do final da Property.  
        //       "{ get; set; }" se torna desnecessário.
        // - ENTÃO, NEM SEMPRE EU COLOCO Property.
        // - Acho que pra mim colocar como Property, deveria ficar sem o "set;", assim:
        //       private readonly SalesWebMvc_Net7Context _context { get; }
        private readonly SalesWebMvc_Net7Context _context;

        public DepartmentService(SalesWebMvc_Net7Context context)
        {
            _context = context;
        }

        // Método pra retornar todos os Departamentos.
        // Este método era FindAll().
        // public List<Department> FindAllAsync();
        //
        // Ele era SÍNCRONO. Agora, passou a ser ASSÍNCRONA.
        //
        // Operações de Banco de Dados, de disco ou de rede, são operações lentas.
        // Então, esta operações elas devem ser ASSÍNCRONAS.
        //
        // Como declarar uma operação assíncrona?
        // - palavrinha "async" antes do TipoDeRetorno,
        // - depois Task<TipoDeRetorn>
        // - palavrinha "await" antes do que vai ser retornado da função;
        //    no caso o retorno é uma "expressão lambda, que retorna um Lista ordenada por Nome", e depois o 
        //
        public async Task<List<Department>> FindAllAsync()
        {

            // Poderia só retornar a lista. 
            // Mas vamos retornar ela ordenada, no comando a seguir.
            //return _context.Department.ToList();

            // Chamando a função do LINQ, a OrderBy(), vamos retornar a lista ORDENADA por Nome. 
            // Para os Departamentos ficarem ordenados.
            // retorna a tabela "Department" do _context (contexto de BD do Entity Framework), convertida para Lista.
            //
            // "await" (espera a resposta da chamada ASSÍNCRONA, que no caso é o ToListAsync().
            //    Este "await" é pra avisar o compilador que isso aqui (.ToListAsync();) é uma chamada ASSINCRONA.
            //    E quando a operação terminar, aí sim:
            //    o controle volta para o ponto onde ela foi chamada, e a minha aplicação continua.
            //
            // No caso aqui, o ToList() é uma operação SÍNCRONA (a aplicação fica bloqueada executando o
            // ToList(). ToList() é do C#.
            //   Porém, existe uma outra versão do ToList().
            //   É a ToListAsync().
            //       Só que é esta operação ToListAsync() ela não é do Linq. Ela é do (namespace) Microsoft.EntityFrameworkCore.
            //       Então prá isso funcionar (eu usar a ToListAsync) eu tenho que colocar o namespace Microsoft.EntityFrameworkCore.
            //       Assim:
            //             using Microsoft.EntityFrameworkCore;  
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
