﻿using SalesWebMvc_.Net7.Models; // É onode está a nossa Classe"SalesWebMvc_Net7Context".    // Context é o nosso contexto de BD, no C#.
using System.Linq; // Porque vamos precisar do Linq: vamos usar o Função (Método) OrderBy do Linq. 
using System.Collections.Generic; // Para importar a Lista (Classe List).
  
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
        public List<Department> FindAll()
        {

            // Poderia só retornar a lista. 
            // Mas vamos retornar ela ordenada, no comando a seguir.
            //return _context.Department.ToList();


            // Chamando a função do LINQ, a OrderBy(), vamos retornar a lista ORDENADA por Nome. 
            // Para os Departamentos ficarem ordenados.
            // retorna a tabela "Department" do _context (contexto de BD do Entity Framework), convertida para Lista.
            return _context.Department.OrderBy(x => x.Name).ToList();
        }
    }
}