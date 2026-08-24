using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Threading.Tasks;

namespace SenacFlix.UI.ViewComponents
{
    /*
    View Component responsável por exibir o Hero Banner na página inicial.
    Faz uma requisição para a API para obter o filme em destaque e o exibe no banner.
    */
    public class HeroBannerViewComponent : ViewComponent
    {
        //Variável responsável por armazenar a instância de ApiCliente
        private readonly ApiCliente _api;

        //Construtor que recebe a instância de ApiCliente via Injeção de Dependência
        public HeroBannerViewComponent(ApiCliente api)
        {
            //Atribui a instância de ApiCliente à variável _api
            _api = api;
        }

        /*
        Método responsável por invocar o View Component que exibe o Hero Banner na página inicial.
        Faz uma requisição para a API para obter o filme em destaque e o exibe no banner.
        */
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Obtém o filme em destaque da API
            var respDestaque = await _api.GetAsync<FilmeViewModel>("/api/Filmes/destaque");

            //Verifica se obteve sucesso e se tem dados
            if (respDestaque.Sucesso && respDestaque.Dados != null)
            {
                //Retorna a view com o filme em destaque
                return View(respDestaque.Dados);
            }
            //Retorna a view sem filme em destaque caso não tenha encontrado
            return View((FilmeViewModel)null);
        }
    }
}
