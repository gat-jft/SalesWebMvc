using Microsoft.EntityFrameworkCore;
using SalesWebMvc_.Net7.Data;

namespace SalesWebMvc_.Net7
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // A variável "builder" recebeu (aponta) para um WebApplicationBuilder (Construtor, ou melhor dizendo, aponta para um "objeto Construtor de uma Aplicação Web e Serviço Web").
            var builder = WebApplication.CreateBuilder(args);

            // Variável "versaoDoServidorMySql".  Tive que criar esta variável para compor o 2° parâmetro do Método UseMySql (que substituiu o UseSqlServer).    // ServerType (TipoDoServidor) é um tipo enum (0 = MySql). Tive que completar com "MySql" para funcionar.     // 8,0,0 é a versão do MySql Workbench.
            var mySqlServerVersion = Microsoft.EntityFrameworkCore.ServerVersion.Create(8, 0, 0, Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);

            // 1- Uma instância DbContext (AddDbContext) representa uma sessão com o banco de dados e pode ser usada para consultar e salvar instâncias de suas entidades. DbContext é uma combinação dos padrões Unit Of Work e Repository.      // AddDbContext é a Classe que adiciona uma Nova Conexão com o MySql (no caso).
            // 2- MySql(..., ..., ...) é um método DE EXTENSÃO, para eu configurar a conexão com o MySql.  // MySql() é um MétodoDeExtensão da Classe WebApplicationBuilder (classe que é um Construtor para Aplicações e Serviços Web).   // MySql(stringDeConexão, versãoDoBancoDeDados, Action Que Retorna Qual Assembly/Projeto será usado).    Action (3° parâmetro) é uma FUNÇÃO com Linq, que recebe uma Expressão Lambda (função anônima). Ela vai retornar do Construtor (objeto buider) qual será o Assembly (Projeto) em que vou fazer as Migration (é um script específico para gerar e versionar a Base de Dados pra gente): A cada sessão com o BD tenho que criar uma nova Migration ("Add-Migration NomeDaMigration"). Geralmente é em Tools / NuGet Packages Manager / Console.        // Mas, o que é um extension method? - É um método que eu adiciono à um Tipo (class ou struct) de dado qualquer, para extender a funcionalidade deste TipoDeDado (MAS SEM HERANÇA).      // Por exemplo, posso extender (acrescentar mais métodos) métodos (a funcionalidade) da Classe String, ou da struct Int, ou da Classe Funcionário por exemplo.  // Basta eu criar um novo Tipo static, e colocar o novo método neste Tipo. Só que no parâmetro deste Método, eu tenho que fazer uma referência (colocar a palavrinha "this") para o próprio TipoQueEuVouExtender.    // Exemplo: public string Cut(this String thisObj, int x);.  Neste caso, estou criando um Método chamado Cut(), que não é próprio (não existe ou existia) da Classe String. E ele não herdará nada da Classe String.      Assim, quando eu CHAMAR o método Cut() de um objeto Tipo String, e passar um String para o Cut, ele irá cortar a String passada no tamanho de x (o 2° parâmetro de Cut()). 
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