using System.Collections.Generic;



namespace SalesWebMvc_.Net7.Models.ViewModels
{
    public class SellerFormViewModel
    {
        // Esta Classe é a Classe que vai conter os dados para o formulário do CADASTRO DE VENDEDOR.
        //
        // Quais vão ser os DADOS NECESSÁRIOS pra uma tela de Cadastro de Vendedor?
        // - Vendedor, e uma Lista<Departamentos>.

        // - Vai ter que ter 1 Vendedor:
        public Seller Seller { get; set; }

        // - Além disso, vai ter que ter também uma listinha de Departamentos.   // Pra que eu possa criar a caixinha, pra que eu possa selecionar o Departamento do Vendedor.
        // - Vou chamar minha listinha de "Departments".
        public ICollection<Department> Departments { get; set; }


        //OBS:
        //- É importante eu usar os nomes CORRETOS:
        //     Seller e Departments (Departments no PLURAL).
        //- Isso ajuda o framework a RECONHECER OS DADOS.
        //     Aí, na hora de fazer a conversão dos DADOS HTTP para OBJETO,
        //     ele já sabe fazer automaticamente.
        //- O nome Seller (no singular), e pq é 1 Vendedor.
        //- O nome Departments (no plural), é pq é uma Lista com vários Departmentos.
    }
}
