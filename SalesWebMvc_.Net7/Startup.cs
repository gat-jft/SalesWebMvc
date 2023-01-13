using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Data;
using SalesWebMvc_.Net7.Models;

namespace SalesWebMvc_.Net7
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // TRADUÇÃO:
        //    Este método é chamado pelo tempo de execução.
        //    Use este método para adicionar serviços ao contêiner (aplicativo Mvc).
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Variável para o configuração do MySql neste Serviço (AddDbContext). Injeção de Dependência: Toda vez que eu criar um  
            var mySqlVersion = Microsoft.EntityFrameworkCore.ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);

            // AddDbContext:
            //  Use este método ao usar injeção de dependência em seu aplicativo, como com ASP.NET Core.
            //  Para aplicativos que não usam injeção de dependência, considere criar <see cref = "DbContext"/> instâncias diretamente com seu construtor. O método <see cref= "DbContext.OnConfiguring"/> pode então ser substituído para configurar uma string de conexão e outras opções.
            services.AddDbContext<SalesWebMvc_Net7Context>(options => options.UseMySql(Configuration.GetConnectionString("SalesWebMvc_Net7Context"), mySqlVersion, options => options.MigrationsAssembly("SalesWebMvc_.Net7")));

            // AddMvc().SetCompatibilityVersion está OBSOLETO.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Coloquei esta opção, para corrigir o erro de gerar a ROTA (no navegador).
            // Assim corrigir O VERDINHO (erro) do app.Mvc(routes => routes ...) no método Configure.
            services.AddMvc(options => options.EnableEndpointRouting = false);

            // services.AddScoped<Classe que representa o Serviço>();
            //   Método AddScoped, chamado do objeto services.
            //   Serve para eu registrar um Serviço (QUE EU CRIEI), no sistema da injeção de dependência.
            //   Registra um Serviço (no caso o SeedService) COM ESCOPO, para injeção de dependência.
            //      É injetado o serviço SeedingService (criado por nós), a cada requisição.
            //      AddScoped tem a ver com o tipo de vida do serviço TAMBÉM.
            //         A cada nova requisição (solicitação) deste Serviço, o anterior é LIMPADO DOS
            //         RECURSOS NÃO GERENCIDOS pelo Método Dispose().
            //
            // Com este comando, a gente REGISTRA O NOSSO SERVIÇO (SeedingService.cs) NO SISTEMA DE INJEÇÃO DE DEPENDÊNCIA DA APLICAÇÃO. 
            services.AddScoped<SeedingService>();

            // Não tem este comando no Startup original. Qq erro, retirá-lo.
            //    - Adiciona serviços para controladores ao
            //      <consulte cref = "IServiceCollection"/> especificado.Este método não registrará
            //      serviços usados para páginas.
            //    - Este método configura os serviços MVC para os recursos comumente usados com controladores
            //      com exibições.Isso combina os efeitos de
            //      <see cref = "MvcCoreServiceCollectionExtensions.AddMvcCore(IServiceCollection)"/>,
            //    - Para adicionar serviços para páginas, chame
            //      <see cref = "AddRazorPages(IServiceCollection)"/>
            services.AddControllersWithViews();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, SeedingService seedingService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Com esta operação (Seed()) eu vou popular a minha base de dados, caso ela não esteja populada ainda.
                seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting(); // Não veio nesta Classe Startup. Eu adicionei. Se der erro, retirar então.
            app.UseCookiePolicy();
            app.UseAuthorization();

            app.UseMvc(routes => routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}

