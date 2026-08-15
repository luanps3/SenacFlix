using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;

namespace SenacFlix.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Operador")]
    public class DashboardController : Controller
    {
        private readonly ApiCliente _api;

        public DashboardController(ApiCliente api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            // Os dados da API são populados no ViewModel e em seguida passados para a View
            var resposta = await _api.GetAsync<DashboardEstatisticasViewModel>("/api/Estatisticas/dashboard");

            if (!resposta.Sucesso || resposta.Dados == null)
            {
                ViewBag.ErroDashboard = resposta.Mensagem ?? "Não foi possível carregar os dados do dashboard.";
                return View(new DashboardEstatisticasViewModel());
            }

            return View(resposta.Dados);
        }
    }
}
