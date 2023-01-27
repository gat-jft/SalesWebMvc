using SalesWebMvc_.Net7.Models; // Para o nosso Context "SalesWebMvc_Net7Context".    // Context é o nosso contexto de BD, no C#.
  
namespace SalesWebMvc_.Net7.Services
{
    public class DepartmentService
    {
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

        public List<Department> FindAll()
        {

            // Poderia só retornar a lista. 
            // Mas vamos retornar ela ordenada, no comando a seguir.
            //return _context.Department.ToList();


            // Chamando a função do LINQ, a OrderBy(), vamos retornar a lista ORDENADA POR NOME. 
            // Para os Departamentos ficarem ordenados.
            // retorna a tabela "Department" do _context (contexto de BD do Entity Framework), convertida para Lista.
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}
