using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View(model);
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
