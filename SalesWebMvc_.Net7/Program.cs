using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Data;

namespace SalesWebMvc_.Net7
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // A vari�vel "builder" recebeu (aponta) para um WebApplicationBuilder (Construtor, ou melhor dizendo, aponta para um "objeto Construtor de uma Aplica��o Web e Servi�o Web").
            var builder = WebApplication.CreateBuilder(args);

            // Vari�vel "versaoDoServidorMySql".  Tive que criar esta vari�vel para compor o 2� par�metro do M�todo UseMySql (que substituiu o UseSqlServer).    // ServerType (TipoDoServidor) � um tipo enum (0 = MySql). Tive que completar com "MySql" para funcionar.     // 8,0,0 � a vers�o do MySql Workbench.
            var mySqlServerVersion = Microsoft.EntityFrameworkCore.ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);

            // 1- Uma inst�ncia DbContext (AddDbContext) representa uma sess�o com o banco de dados e pode ser usada para consultar e salvar inst�ncias de suas entidades. DbContext � uma combina��o dos padr�es Unit Of Work e Repository.      // AddDbContext � a Classe que adiciona uma Nova Conex�o com o MySql (no caso).
            // 2- MySql(..., ..., ...) � um m�todo DE EXTENS�O, para eu configurar a conex�o com o MySql.  // MySql() � um M�todoDeExtens�o da Classe WebApplicationBuilder (classe que � um Construtor para Aplica��es e Servi�os Web).   // MySql(stringDeConex�o, vers�oDoBancoDeDados, Action Que Retorna Qual Assembly/Projeto ser� usado).    Action (3� par�metro) � uma FUN��O com Linq, que recebe uma Express�o Lambda (fun��o an�nima). Ela vai retornar do Construtor (objeto buider) qual ser� o Assembly (Projeto) em que vou fazer as Migration (� um script espec�fico para gerar e versionar a Base de Dados pra gente): A cada sess�o com o BD tenho que criar uma nova Migration ("Add-Migration NomeDaMigration"). Geralmente � em Tools / NuGet Packages Manager / Console.        // Mas, o que � um extension method? - � um m�todo que eu adiciono � um Tipo (class ou struct) de dado qualquer, para extender a funcionalidade deste TipoDeDado (MAS SEM HERAN�A).      // Por exemplo, posso extender (acrescentar mais m�todos) m�todos (a funcionalidade) da Classe String, ou da struct Int, ou da Classe Funcion�rio por exemplo.  // Basta eu criar um novo Tipo static, e colocar o novo m�todo neste Tipo. S� que no par�metro deste M�todo, eu tenho que fazer uma refer�ncia (colocar a palavrinha "this") para o pr�prio TipoQueEuVouExtender.    // Exemplo: public string Cut(this String thisObj, int x);.  Neste caso, estou criando um M�todo chamado Cut(), que n�o � pr�prio (n�o existe ou existia) da Classe String. E ele n�o herdar� nada da Classe String.      Assim, quando eu CHAMAR o m�todo Cut() de um objeto Tipo String, e passar um String para o Cut, ele ir� cortar a String passada no tamanho de x (o 2� par�metro de Cut()). 
            builder.Services.AddDbContext<SalesWebMvc_Net7Context>(options =>
               options.UseMySql(builder.Configuration.GetConnectionString("SalesWebMvc_Net7Context"), mySqlServerVersion, builder =>
               builder.MigrationsAssembly("SalesWebMvc_.Net7")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}