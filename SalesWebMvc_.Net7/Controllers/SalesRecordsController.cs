using Microsoft.AspNetCore.Mvc;
using SalesWebMvc_.Net7.Services;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc_.Net7.Controllers
{
    public class SalesRecordsController : Controller
    {
        // Prá que eu possa utilizar meu serviço, eu tenho que declarar a dependência dele aqui (neste controlador).
        public readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // SimpleSearch = Busca simples.
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            // Aqui, na verdade eu estou TESTANDO a Negação como true, visto que o if é:
            // if (true) { }.
            //
            // Se a DataMínima NÃO possui valor:
            if (!minDate.HasValue)
            {
                // Eu vou atribuir um valor padrão para ela, por exemplo,
                // o Dia 1° de Janeiro do Ano ATUAL.
                //
                // O formato de um DateTime é o que eu coloco nos () dele:
                // (year,MM, dd)
                // 
                // Como year eu quero o Ano ATUAL (Now) .Year.
                // O MÊS vai se 1. E o dia vai ser 1 também.
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            // Vou fazer a mesma coisa prá DataMáxima. Se ele não for informada,
            // eu vou pegar a DataAtual.
            if (!maxDate.HasValue)
            {               
                maxDate = DateTime.Now;
            }


            
            // Quando eu clicava no botão "Submit" da View SalesRecord OU no botão "Filter" da View SimpleSearch,
            // o formulário funcionava. Só que as Datas não ficavam REGISTRADAS nas
            // caixinhas de DataMínima e DataMáxima.
            //
            // Para que os valores fiquem nas Caixinhas permanentemente, o CONTROLADOR tem que
            // enviar os valores lá prá View (SimpleSearch no caso).
            // Como enviar: 
            // - Criando aqui no Controlador 2 Dicionários (1 para chave minDate, outro para a chave
            //   maxDate.
            // - Aí, lá na View de SimpleSearch, no <input> de data, eu vou acrescentar:
            //   value=@ViewData["minDate"].
            // - Acrescentar também na View de SimpleSearch, no <input> de data, o
            //   value=@ViewData["maxDate"].    
            //
            // ViewData, na chave minDate, vai receber o minDate.Value, no Formato "yyyy-MM-dd".
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");           
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            
            // Enviando a Lista (result).
            return View(result);
        }

        // ESta operação é muito parecida com a anterior.
        //
        // Então, copiei a anterior, colei aqui, e alterei pouca coisa?
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            // Aqui, na verdade eu estou TESTANDO a Negação como true, visto que o if é:
            // if (true) { }.
            //
            // Se a DataMínima NÃO possui valor:
            if (!minDate.HasValue)
            {
                // Eu vou atribuir um valor padrão para ela, por exemplo,
                // o Dia 1° de Janeiro do Ano ATUAL.
                //
                // O formato de um DateTime é o que eu coloco nos () dele:
                // (year,MM, dd)
                // 
                // Como year eu quero o Ano ATUAL (Now) .Year.
                // O MÊS vai se 1. E o dia vai ser 1 também.
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            // Vou fazer a mesma coisa prá DataMáxima. Se ele não for informada,
            // eu vou pegar a DataAtual.
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }



            // Quando eu clicava no botão "Submit" da View SalesRecord OU no botão "Filter" da View SimpleSearch,
            // o formulário funcionava. Só que as Datas não ficavam REGISTRADAS nas
            // caixinhas de DataMínima e DataMáxima.
            //
            // Para que os valores fiquem nas Caixinhas permanentemente, o CONTROLADOR tem que
            // enviar os valores lá prá View (SimpleSearch no caso).
            // Como enviar: 
            // - Criando aqui no Controlador 2 Dicionários (1 para chave minDate, outro para a chave
            //   maxDate.
            // - Aí, lá na View de SimpleSearch, no <input> de data, eu vou acrescentar:
            //   value=@ViewData["minDate"].
            // - Acrescentar também na View de SimpleSearch, no <input> de data, o
            //   value=@ViewData["maxDate"].    
            //
            // ViewData, na chave minDate, vai receber o minDate.Value, no Formato "yyyy-MM-dd".
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");

            // Eu vou chamar esta operação "await _salesRecordService.FindByDateGroupingsync(minDate, maxDate);" que é ASSÍNCRONA.
            // Quando vier o resultado (var result), eu vou encaminhar o resultado para a View. 
            var result = await _salesRecordService.FindByDateGroupingsync(minDate, maxDate);

            // Enviando a Lista (result).
            return View(result);
        }

    }
}
