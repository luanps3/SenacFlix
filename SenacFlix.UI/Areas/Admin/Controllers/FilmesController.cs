// Nome do arquivo: FilmesController.cs
// Objetivo: Gerenciamento de filmes pelo admin (CRUD)
// Camada: UI

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SenacFlix.UI.Infraestrutura;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Operador")]
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
                TempData["Erro"] = "Filme não encontrado.";
                return RedirectToAction(nameof(Index));
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

        [HttpPost]
        public async Task<IActionResult> Criar(FilmeEdicaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PreencherDropdowns(model);
                return View(model);
            }

            // Processa a imagem de capa (Upload ou Url)
            string? capaUrl = null;
            if (model.TipoCapa == "Url" && !string.IsNullOrWhiteSpace(model.NovaImagemCapaUrlInfo))
            {
                capaUrl = model.NovaImagemCapaUrlInfo;
            }
            else if (model.NovaImagemCapa != null)
            {
                capaUrl = await _upload.SalvarArquivoAsync(model.NovaImagemCapa, "capas");
            }

            // Processa a imagem de banner (Upload ou Url)
            string? bannerUrl = null;
            if (model.TipoBanner == "Url" && !string.IsNullOrWhiteSpace(model.NovaImagemBannerUrlInfo))
            {
                bannerUrl = model.NovaImagemBannerUrlInfo;
            }
            else if (model.NovaImagemBanner != null)
            {
                bannerUrl = await _upload.SalvarArquivoAsync(model.NovaImagemBanner, "banners");
            }

            var dto = new
            {
                model.Titulo,
                model.Descricao,
                model.AnoLancamento,
                model.Duracao,
                model.Diretor,
                model.Elenco,
                ImagemCapaUrl = capaUrl,
                ImagemBannerUrl = bannerUrl,
                model.TrailerYoutubeUrl,
                model.VideoYoutubeUrl,
                model.CategoriaId,
                model.ClassificacaoIndicativaId
            };

            var resposta = await _api.PostAsync<FilmeViewModel, object>("/api/Filmes", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Filme criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", resposta.Mensagem);
            await PreencherDropdowns(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var resposta = await _api.GetAsync<FilmeViewModel>($"/api/Filmes/{id}");
            if (!resposta.Sucesso || resposta.Dados == null)
            {
                TempData["Erro"] = "Filme não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var f = resposta.Dados;
            var model = new FilmeEdicaoViewModel
            {
                Id = f.Id,
                Titulo = f.Titulo,
                Descricao = f.Descricao,
                AnoLancamento = f.AnoLancamento,
                // f.Duracao ja e int (minutos), nenhuma conversao necessaria
                Duracao = f.Duracao,
                Diretor = f.Diretor,
                Elenco = f.Elenco,
                TrailerYoutubeUrl = f.TrailerYoutubeUrl,
                VideoYoutubeUrl = f.VideoYoutubeUrl,
                CategoriaId = f.CategoriaId,
                ClassificacaoIndicativaId = f.ClassificacaoIndicativaId,
                ImagemCapaUrlAtual = f.ImagemCapaUrl,
                ImagemBannerUrlAtual = f.ImagemBannerUrl
            };

            await PreencherDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(FilmeEdicaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PreencherDropdowns(model);
                return View(model);
            }

            string? capaUrl = model.ImagemCapaUrlAtual;
            string? bannerUrl = model.ImagemBannerUrlAtual;

            if (model.TipoCapa == "Url" && !string.IsNullOrWhiteSpace(model.NovaImagemCapaUrlInfo))
            {
                capaUrl = model.NovaImagemCapaUrlInfo;
            }
            else if (model.NovaImagemCapa != null)
            {
                capaUrl = await _upload.SalvarArquivoAsync(model.NovaImagemCapa, "capas");
            }

            if (model.TipoBanner == "Url" && !string.IsNullOrWhiteSpace(model.NovaImagemBannerUrlInfo))
            {
                bannerUrl = model.NovaImagemBannerUrlInfo;
            }
            else if (model.NovaImagemBanner != null)
            {
                bannerUrl = await _upload.SalvarArquivoAsync(model.NovaImagemBanner, "banners");
            }

            var dto = new
            {
                model.Id,
                model.Titulo,
                model.Descricao,
                model.AnoLancamento,
                model.Duracao,
                model.Diretor,
                model.Elenco,
                ImagemCapaUrl = capaUrl,
                ImagemBannerUrl = bannerUrl,
                model.TrailerYoutubeUrl,
                model.VideoYoutubeUrl,
                model.CategoriaId,
                model.ClassificacaoIndicativaId
            };

            var resposta = await _api.PutAsync<FilmeViewModel, object>($"/api/Filmes/{model.Id}", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Filme atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", resposta.Mensagem);
            await PreencherDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Filmes/{id}/desativar");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Filme desativado.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _api.PutAsync<object, object>($"/api/Filmes/{id}/reativar", new { });
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Filme reativado com sucesso.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirPermanente(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Filmes/{id}/permanente");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Filme excluído permanentemente.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
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

            // Busca as classificacoes indicativas disponiveis utilizando ViewModel fortemente tipado.
            // Uso de ClassificacaoViewModel garante desserializacao correta sem dynamic/JsonElement.
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
