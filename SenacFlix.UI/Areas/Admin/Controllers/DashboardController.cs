// Nome do arquivo: DashboardController.cs
// Objetivo: Controller inicial da area administrativa — carrega estatisticas via ApiCliente (server-side)
// Camada: UI
// Como participa: Chama a API internamente usando HttpClient (server-to-server) e passa o
//                 resultado como JSON inline para a View. Isso evita problemas de CORS e URL
//                 que ocorriam quando a View tentava fazer fetch() diretamente da API a partir
//                 do browser (URL relativa ao MVC e nao a API).

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Threading.Tasks;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Operador")]
    public class DashboardController : Controller
    {
        private readonly ApiCliente _api;

        public DashboardController(ApiCliente api)
        {
            _api = api;
        }

        // ── Index: busca estatísticas no servidor e passa para a View ────────────
        // A chamada HTTP ocorre aqui (server-side), eliminando problemas de CORS e
        // de URL relativa que afetavam o fetch() do browser.
        public async Task<IActionResult> Index()
        {
            // Chama a API diretamente do servidor (HttpClient com BaseAddress configurado)
            var resposta = await _api.GetAsync<DashboardEstatisticasViewModel>("/api/Estatisticas/dashboard");

            if (!resposta.Sucesso || resposta.Dados == null)
            {
                // Passa uma mensagem de erro para a View exibir
                ViewBag.ErroDashboard = resposta.Mensagem ?? "Não foi possível carregar as estatísticas.";
                return View(new DashboardEstatisticasViewModel());
            }

            // Passa o modelo preenchido para a View
            return View(resposta.Dados);
        }
    }
}
