using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Operador")]
    public class CategoriasController : Controller
    {
        private readonly ApiCliente _api;


        public CategoriasController(ApiCliente api)
        {
            _api = api;
        }

        // Este index não possui verbos HTTP , pois é o padrão do MVC. O verbo HTTP GET é o padrão para ações de exibição de páginas.
        public async Task<IActionResult> Index()
        {
            //Coalescência Nula (??) - Se a resposta.Dados for nula, será retornada uma lista vazia de CategoriaViewModel   
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
                // nameof(Index) é uma forma de referenciar o nome do método Index de forma segura, evitando erros de digitação.
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", resposta.Mensagem ?? "Erro ao criar categoria.");
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

            ModelState.AddModelError("", resposta.Mensagem ?? "Erro ao atualizar categoria.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Categorias/{id}/desativar");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria desativada com sucesso!";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _api.PutAsync<object, object>($"/api/Categorias/{id}/reativar", new{ });
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria reativada com sucesso!";
            else
                TempData["Erro"] = resposta.Mensagem;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirPermanentemente(int id)
        {
            var resposta = await _api.DeleteAsync<object>($"/api/Categorias/{id}/permanente");
            if (resposta.Sucesso)
                TempData["Sucesso"] = "Categoria excluída permanentemente!";
            else
                TempData["Erro"] = resposta.Mensagem;
            return RedirectToAction(nameof(Index));
        }

    }
}
