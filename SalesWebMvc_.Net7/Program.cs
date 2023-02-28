using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SalesWebMvc_.Net7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // IMPORTANTE:
            // Este MEU projeto foi feito inicialmente no .Net7.
            // Só que ele não tem mais a Classe Startup.
            // E como dava dando muitos erros, porque o professor usava o .NET2,
            // eu adicionei a Startup novamente. E fiz as adaptações do
            // Program com a Startup, de acordo com o .NET anterior (como o
            // .NET5, eu acho!) que tinha a Classe Startup.cs.
            //
            // Aí, funcionou.
            // ERROS que deram antes de eu adicionar a Startup:
            // - Não conseguia colocar o meu Serviço SeedingService.cs no
            // Sistema de Injeção de Dependência.
            // Porque, na Program, ele não consegui manipular o Método
            // Configure, no objeto builder.

            CreateWebHostBuilder(args).Build().Run(); 
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}