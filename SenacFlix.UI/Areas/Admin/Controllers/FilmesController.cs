using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SenacFlix.UI.Infraestrutura;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Operador")]
    public class FilmesController : Controller
    {
        private readonly ApiCliente _api;
        private readonly ServicoUpload _upload;

        public FilmesController(ApiCliente api, ServicoUpload upload)
        {
            _api = api;
            _upload = upload;
        }

        public async Task<IActionResult> Index()
        {
            var resposta = await _api.GetAsync<IEnumerable<FilmeViewModel>>("/api/Filmes/todos");
            return View(resposta.Dados ?? new List<FilmeViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Visualizar(int id)
        {
            var resposta = await _api.GetAsync<FilmeViewModel>($"/api/Filmes/{id}");

            if (!resposta.Sucesso || resposta.Dados == null)
            {
                ViewBag.ErroVisualizar = resposta.Mensagem ?? "Não foi possível carregar os dados do filme.";
                return View(new FilmeViewModel());
            }

            return View(resposta.Dados);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var model = new FilmeEdicaoViewModel();
            await PreencherDropdowns(model);
            return View(model);
        }

        private async Task PreencherDropdowns(FilmeEdicaoViewModel model)
        {
            var categorias = await _api.GetAsync<IEnumerable<CategoriaViewModel>>("/api/Categorias/todas");
            if (categorias.Sucesso && categorias.Dados != null)
            {
                model.CategoriasDisponiveis = categorias.Dados.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nome
                });
            }

            var classificacoes = await _api.GetAsync<IEnumerable<ClassificacaoViewModel>>("/api/Classificacoes");
            if (classificacoes.Sucesso && classificacoes.Dados != null)
            {
                model.ClassificacoesDisponiveis = classificacoes.Dados.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nome
                });
            }
        }

    }
}
