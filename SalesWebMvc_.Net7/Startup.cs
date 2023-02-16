using System.Globalization; // Para formatação de números e datas
using Microsoft.AspNetCore.Localization; // Para definir a localização da nossa Aplicação. Vamos definir como sendo a dos Estados Unidos, que é a mais genérica de todas.  
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Data;
using SalesWebMvc_.Net7.Models;
using SalesWebMvc_.Net7.Services;



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

            // AddMvc().SetCompatibilityVersion está OBSOLETO.
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Variável para o configuração do MySql neste Serviço (AddDbContext). Injeção de Dependência: Toda vez que eu criar um  
            var mySqlVersion = Microsoft.EntityFrameworkCore.ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);

            // AddDbContext:
            //  Use este método ao usar injeção de dependência em seu aplicativo, como com ASP.NET Core.
            //  Para aplicativos que não usam injeção de dependência, considere criar <see cref = "DbContext"/> instâncias diretamente com seu construtor. O método <see cref= "DbContext.OnConfiguring"/> pode então ser substituído para configurar uma string de conexão e outras opções.
            services.AddDbContext<SalesWebMvc_Net7Context>(options => options.UseMySql(Configuration.GetConnectionString("SalesWebMvc_Net7Context"), mySqlVersion, options => options.MigrationsAssembly("SalesWebMvc_.Net7")));


            // Coloquei esta opção, para corrigir o erro de gerar a ROTA (no navegador).
            // Assim corrigir O VERDINHO (erro) do app.Mvc(routes => routes ...) no método Configure.
            services.AddMvc(options => options.EnableEndpointRouting = false);


            // ESTE COMANDO NÃO EXISTE NO Startup.cs ORIGINAL. Qq erro, retirá-lo.
            //    - Adiciona serviços para controladores ao
            //      <consulte cref = "IServiceCollection"/> especificado.Este método não registrará
            //      serviços usados para páginas.
            //    - Este método configura os serviços MVC para os recursos comumente usados com controladores
            //      com exibições.Isso combina os efeitos de
            //      <see cref = "MvcCoreServiceCollectionExtensions.AddMvcCore(IServiceCollection)"/>,
            //    - Para adicionar serviços para páginas, chame
            //      <see cref = "AddRazorPages(IServiceCollection)"/>
            services.AddControllersWithViews();

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
            services.AddScoped<SellerService>(); // Registrei no sistema de injeção de dependência. Agora, nosso Serviço (Classe) pode ser injetado em outras Classes. Não por mim, mas automaticamente pelo framework. Assim:   // Qualquer Classe que eu colocar no parêmetro construtor dela, que ela recebe um SellerService, toda vez que eu instanciar um objeto do tipo desta Classe, receberá um objeto SellerService automaticamente vinculado à ele.  
            services.AddScoped<DepartmentService>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, SeedingService seedingService)
        {
            // Se eu colocar um parâmetro no Configure (no caso eu adicionei o Seeding Service seeding), e
            // se esta Classe estiver registrada no sistema de injeção de depenência (estiver num AddScoped<> por exemplo, 
            // no método ConfigureServices acima), AUTOMATICAMENTE vai ser resolvido uma instância
            // deste objeto.
            // 
            // Assim, durante toda a minha aplicação, eu terei o objeto seedingService sempre na
            // na minha aplicação (global). Para que a qualquer requisição (ex. Update de um Seller),
            // meu BD se não tiver nenhum dado, este serviço (objeto do tipo SeedingService) irá 
            // povoar as tabelas do BD, com alguns dados definidos neste Serviço.

            // app é um objeto da Classe IApplicationBuilder (que Define uma classe que fornece os
            // mecanismos para configurar o pipeline de solicitação de um aplicativo.

            // env é um objeto da Classe IHostEnvironment (que fornece informações sobre o ambiente que o 
            // aplicativo está sendo executado).
            // Como este objeto env está nos parâmetros deste Configure(), AUTOMATICAMENTE é 
            // ele é criado. Trazendo consigo as informações de qual ambiente de execução
            // Existem 2 tipos de ambientes de execução, que uma aplicação pode estar sendo executada:
            // - Ambiente de Desenvolvimento:
            //       Ele esta sendo executado numa IDE (ex. Visual Studio);
            //       // O raciocínio aqui é muito simples: Eu estou no perfil de desenvolvimento (IDE).
            // - Ambiente de Produção:
            //       Esse aplicativo já tiver publicado.

            // Variáveis "enUs" e "localizationOption" e "app.UseRequestLocalization(localizationOptions)":
            // Estes Comandos em conjunto, são para configurar a definição do locale da
            // aplicação, como sendo dos Estados Unidos.
            // 
            // var enUs, para instanciarmos (criamos) uma Cultura (CultureInfo). Ela vai ser a dos Estados Unidos.
            var enUS = new CultureInfo("en-US");
            var localizationOptions = new RequestLocalizationOptions
            {
                // Este objeto da Classe (tipo) RequestLocalizationOptions, vai ter os seguintes dados
                // TRIBUTOS:
                // - DefaultRequestCulture,
                // - SupportedCultures e
                // - SupportedUICultures.

                // 1 -  Qual vaI ser o locale padrão da minha Aplicação. 
                DefaultRequestCulture = new RequestCulture(enUS),

                // 2 - Quais são os locales possíveis da minha Aplicação?
                //     - Eu vou criar então, um new List<CultureInfo>.
                //     - Aí, eu vou colocar nesta Lista, somente o meu objeto enUs.
                SupportedCultures = new List<CultureInfo> { enUS },

                // 3 - Este é o último Atributo (da Classe RequestLocalizationOptions) que nós vamos setar.
                SupportedUICultures = new List<CultureInfo> { enUS }
            };

            // Definimos as opções de localização (variáveis enUs e localizationOptions), 
            // agora vamos usar este comando abaixo, passando este objeto
            // (localizationOptions) nosso de localização:
            app.UseRequestLocalization(localizationOptions);






            if (env.IsDevelopment())
            {
                // O que ambiente de desenvolvimento?
                // - É um dos 2 ambientes que uma aplicação está sendo executada:
                //     - IDE, como aqui no Visual Studio, ou,
                //     - já tiver sido publicado. // Para publicar (não executar + pelo VS) a aplicação, digito
                //                                // - "publicar" na caixinha de Pesquisa no VS.
                //                                // - Depois clico em "Adicionar um perfil de publicação",
                //                                //   // Para implantar o aplicativo no Azure, no IIS, em uma pasta,
                //                                //   // ou em outro host.
                //                                //   //
                //                                // - Ou posso clicar em Compilação / Publicar. 
                //  
                //
                // Assim,
                // - toda vez que rodamos (executamos) nossa aplicação, como estamos executano pela
                //   IDE, a aplicação (o objeto app) chama o método UseDeveloperExceptionPage(), que captura
                //   uma Exceção síncrona e assíncrona, de uma pipeline, e gera uma resposta de 
                //   Erro HTML.
                // - O método SeedingService.Seed() é executado.                

                app.UseDeveloperExceptionPage();

                // Com esta operação (Seed()) eu vou popular a minha base de dados, caso ela não esteja populada ainda.
                //
                // Este nossa serviço será executado no Ambiente de Desenvolvimento (IDE).
                // Então, toda vez que eu RODAR minha aplicação (na IDE), ele irá verificar
                // se o BD está vazio ou não. 
                // - Se tiver, ele será povoado.
                seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseRouting(); // Não veio nesta Classe Startup. Eu adicionei. Se der erro, retirar então.
            app.UseCookiePolicy();
            //app.UseAuthorization(); // Não veio na Classe Startup original.

            app.UseMvc(routes => routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));

            //app.UseEndpoints(endpoints => {  // Não veio na Classe Startup original.
            //    endpoints.MapControllers();
            //});
        }
    }
}

