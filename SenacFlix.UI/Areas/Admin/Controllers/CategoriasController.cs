// Nome do arquivo: CategoriasController.cs
// Objetivo: Gerenciamento de categorias pelo admin (CRUD)
// Camada: UI

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Infraestrutura;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Operador")]
    public class CategoriasController : Controller
    {
        private readonly ApiCliente _api;

        public CategoriasController(ApiCliente api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var resposta = await _api.GetAsync<IEnumerable<CategoriaViewModel>>("/api/Categorias/todas");
            return View(resposta.Dados ?? new List<CategoriaViewModel>());
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View(new CategoriaEdicaoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Criar(CategoriaEdicaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new
            {
                model.Nome,
                model.Descricao
            };

            var resposta = await _api.PostAsync<CategoriaViewModel, object>("/api/Categorias", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Categoria criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", resposta.Mensagem ?? "Erro desconhecido.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var resposta = await _api.GetAsync<CategoriaViewModel>($"/api/Categorias/{id}");
            if (!resposta.Sucesso || resposta.Dados == null)
            {
                TempData["Erro"] = "Categoria não encontrada.";
                return RedirectToAction(nameof(Index));
            }

            var c = resposta.Dados;
            var model = new CategoriaEdicaoViewModel
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaEdicaoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new
            {
                model.Nome,
                model.Descricao
            };

            var resposta = await _api.PutAsync<CategoriaViewModel, object>($"/api/Categorias/{model.Id}", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Categoria atualizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", resposta.Mensagem ?? "Erro desconhecido.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Categorias/{id}/desativar");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria inativada com sucesso.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _api.PutAsync<object, object>($"/api/Categorias/{id}/reativar", new { });
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria reativada com sucesso.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirPermanente(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Categorias/{id}/permanente");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria excluída permanentemente.";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }
    }
}
