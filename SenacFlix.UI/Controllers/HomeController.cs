// Nome do arquivo: HomeController.cs
// Objetivo: Controlador para a tela inicial publica da plataforma
// Camada: UI

using Microsoft.AspNetCore.Mvc;

namespace SenacFlix.UI.Controllers
{
    public class HomeController : Controller
    {
        // Acao que renderiza a View Index (Landing Page)
        public IActionResult Index()
        {
            // Se o usuario estiver logado, redireciona para o catalogo
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Catalogo", new { area = "Cliente" });
            }

            return View();
        }

        // Pagina estatica de sobre o projeto (didatico)
        public IActionResult Sobre()
        {
            return View();
        }
    }
}
