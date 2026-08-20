using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.ViewModels;
using SenacFlix.UI.Servicos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly ApiCliente _api;

        public UsuariosController(ApiCliente api)
        {
            _api = api;
        }

        // GET: /Admin/Usuarios
        public async Task<IActionResult> Index()
        {
            var resposta = await _api.GetAsync<IEnumerable<UsuarioViewModel>>("/api/Usuarios");
            
            if (resposta.Sucesso && resposta.Dados != null)
            {
                var usuarios = resposta.Dados.ToList();
                return View(usuarios);
            }

            ViewBag.Erro = resposta.Mensagem ?? "Erro ao carregar os usuários.";
            return View(new List<UsuarioViewModel>());
        }

        // POST: /Admin/Usuarios/Desativar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desativar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Erro"] = "ID de usuário inválido.";
                return RedirectToAction(nameof(Index));
            }

            var resposta = await _api.DeleteAsync<bool>($"/api/Usuarios/{id}");
            
            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Usuário desativado com sucesso.";
            }
            else
            {
                TempData["Erro"] = resposta.Mensagem ?? "Erro ao desativar usuário.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/Usuarios/Ativar/5
        // ValidateAntiForgeryToken serve para evitar ataques do tipo CSRF(Cross-Site Request Forgery)
        // ele cria uma dupla chave e compara a do cliente com a do servidor.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ativar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Erro"] = "ID de usuário inválido.";
                return RedirectToAction(nameof(Index));
            }

            var resposta = await _api.PutAsync<object, bool>($"/api/Usuarios/{id}/ativar", false);
            
            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Usuário ativado com sucesso.";
            }
            else
            {
                TempData["Erro"] = resposta.Mensagem ?? "Erro ao ativar usuário.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
