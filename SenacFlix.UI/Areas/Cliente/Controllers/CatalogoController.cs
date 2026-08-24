// Nome do arquivo: CatalogoController.cs
// Objetivo: Exibir o catalogo de filmes para o cliente
// Camada: UI

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenacFlix.UI.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class CatalogoController : Controller
    {
        private readonly ApiCliente _api;

        public CatalogoController(ApiCliente api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(int? categoriaId = null, string? termo = null)
        {
            var model = new ListaFilmesViewModel
            {
                CategoriaFiltro = categoriaId,
                TermoBusca = termo
            };

            // Busca as categorias para o filtro
            var respCategorias = await _api.GetAsync<List<CategoriaViewModel>>("/api/Categorias");
            if (respCategorias.Sucesso)
                model.Categorias = respCategorias.Dados!;



            // Busca os filmes dependendo do filtro
            SenacFlix.UI.Servicos.ApiResposta<List<FilmeViewModel>> respFilmes;

            if (!string.IsNullOrEmpty(termo))
                respFilmes = await _api.GetAsync<List<FilmeViewModel>>($"/api/Filmes/buscar?termo={termo}");
            else if (categoriaId.HasValue)
                respFilmes = await _api.GetAsync<List<FilmeViewModel>>($"/api/Filmes/categoria/{categoriaId.Value}");
            else
                respFilmes = await _api.GetAsync<List<FilmeViewModel>>("/api/Filmes");

            if (respFilmes.Sucesso)
                model.Filmes = respFilmes.Dados!;

            return View(model);
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            var resposta = await _api.GetAsync<FilmeViewModel>($"/api/Filmes/{id}");
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound();

            var model = resposta.Dados;

            // Verifica se este filme eh favorito do usuario logado
            var favResp = await _api.GetAsync<bool>($"/api/Favoritos/verificar/{id}");
            if (favResp.Sucesso)
                model.EhFavorito = favResp.Dados;

            // Filmes relacionados (mesma categoria, excluindo o atual)
            var respRelac = await _api.GetAsync<List<FilmeViewModel>>($"/api/Filmes/categoria/{model.CategoriaId}");
            if (respRelac.Sucesso && respRelac.Dados != null)
                ViewBag.Relacionados = respRelac.Dados.Where(f => f.Id != id).Take(6).ToList();

            return View(model);
        }

        // ── Player: reproduzir o filme completo ───────────────────
        public async Task<IActionResult> Assistir(int id)
        {
            var resposta = await _api.GetAsync<FilmeViewModel>($"/api/Filmes/{id}");
            if (!resposta.Sucesso || resposta.Dados == null)
                return NotFound();

            var modelo = resposta.Dados;

            // Filmes relacionados (mesma categoria, excluindo o atual)
            var respRelac = await _api.GetAsync<List<FilmeViewModel>>($"/api/Filmes/categoria/{modelo.CategoriaId}");
            if (respRelac.Sucesso && respRelac.Dados != null)
                ViewBag.Relacionados = respRelac.Dados.Where(f => f.Id != id).Take(6).ToList();

            ViewData["FullScreen"] = true;

            return View(modelo);
        }
    }
}
