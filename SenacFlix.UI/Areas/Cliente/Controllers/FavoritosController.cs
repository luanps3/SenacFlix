// Nome do arquivo: FavoritosController.cs
// Objetivo: Listar e gerenciar filmes favoritados pelo cliente
// Camada: UI

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenacFlix.UI.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly ApiCliente _api;

        public FavoritosController(ApiCliente api)
        {
            _api = api;
        }

        /// <summary>
        /// Exibe a lista de filmes favoritados pelo usuario autenticado.
        /// Solicita a lista tipada como FavoritoViewModel para que a View receba
        /// objetos fortemente tipados — sem dynamic, sem JsonElement.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var resposta = await _api.GetAsync<List<FavoritoViewModel>>("/api/Favoritos");
            var model = resposta.Dados ?? new List<FavoritoViewModel>();
            return View(model);
        }

        /// <summary>
        /// Adiciona um filme aos favoritos do usuario autenticado.
        /// Redireciona para a pagina de detalhes do filme ao concluir.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Adicionar(int filmeId)
        {
            var dto = new { FilmeId = filmeId };
            var resposta = await _api.PostAsync<object, object>("/api/Favoritos", dto);

            if (resposta.Sucesso)
                TempData["Sucesso"] = "Filme adicionado aos favoritos!";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction("Detalhes", "Catalogo", new { id = filmeId });
        }

        /// <summary>
        /// Remove um filme dos favoritos do usuario autenticado.
        /// Redireciona para a URL informada (tipicamente a propria lista de favoritos)
        /// ou para a pagina de detalhes do filme caso nenhuma URL seja fornecida.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Remover(int filmeId, string? redirectUrl = null)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Favoritos/{filmeId}");

            if (resposta.Sucesso)
                TempData["Sucesso"] = "Filme removido dos favoritos!";
            else
                TempData["Erro"] = resposta.Mensagem;

            if (!string.IsNullOrEmpty(redirectUrl))
                return Redirect(redirectUrl);

            return RedirectToAction("Index");
        }
    }
}
