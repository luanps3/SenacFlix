using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Models;
using System.Diagnostics;

namespace SenacFlix.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Verifica se o usuário está autenticado
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Catalogo", new { area = "Cliente" });
            }
            return View();
        }
    
    }
}
