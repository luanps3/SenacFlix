// Nome do arquivo: ContaController.cs
// Objetivo: Controlador MVC para Autenticacao
// Camada: UI
// Como participa: Chama a API para validar credenciais, gera Cookie e loga o usuario no navegador

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SenacFlix.UI.Controllers
{
    public class ContaController : Controller
    {
        private readonly ApiCliente _api;

        public ContaController(ApiCliente api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            // Se o ModelState estiver invalido, retorna a View com os erros de validacao
            //ModelState.IsValid é automaticamente populado pelo MVC com base nas anotações de validação no LoginViewModel
            if (!ModelState.IsValid)
                return View(model);

            // Chama a API de login
            var resposta = await _api.PostAsync<LoginRespostaApi, LoginViewModel>("/api/Autenticacao/login", model);

            if (resposta.Sucesso && resposta.Dados != null)
            {
                var dados = resposta.Dados;

                // Salva o Token JWT num Cookie seguro HttpOnly para ser enviado pela ApiCliente nas proximas requisicoes
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = dados.Expiracao
                };
                Response.Cookies.Append("senacflix_token", dados.Token, cookieOptions);

                // Cria os Claims para o Cookie do MVC (para que [Authorize] e User.Identity funcionem)
                // Claim é uma informacao sobre o usuario, como nome, email, roles, etc.
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, dados.NomeUsuario),
                    new Claim(ClaimTypes.Email, dados.Email),
                    new Claim("Token", dados.Token) // Tambem guardado aqui por seguranca/praticidade
                };

                if (!string.IsNullOrEmpty(dados.FotoPerfilUrl))
                {
                    claims.Add(new Claim("FotoPerfil", dados.FotoPerfilUrl));
                }

                // Adiciona as roles
                foreach (var role in dados.Perfis)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.LembrarMe
                };

                // Realiza o SignIn no contexto do MVC
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                // Redireciona dependendo do perfil
                // Se houver um returnUrl válido, redireciona para ele
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                if (dados.Perfis.Contains("Admin") || dados.Perfis.Contains("Operador"))
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                else
                    return RedirectToAction("Index", "Catalogo", new { area = "Cliente" });
            }

            ModelState.AddModelError(string.Empty, resposta.Mensagem);
            return View(model);
        }

        //Metodos httpget geralmente retornam uma view, enquanto os metodos httppost processam os dados enviados pelo usuario e retornam uma view ou redirecionam para outra action
        [HttpGet]
        public IActionResult Registrar()
        {
            return View(new RegistroViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Transforma DateOnly em string yyyy-MM-dd que o JSON aceita melhor
            var dto = new
            {
                model.NomeCompleto,
                model.Email,
                model.Senha,
                model.ConfirmarSenha,
                DataNascimento = model.DataNascimento.ToString("yyyy-MM-dd")
            };

            var resposta = await _api.PostAsync<object, object>("/api/Autenticacao/registrar", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Conta criada com sucesso! Faça login.";
                return RedirectToAction(nameof(Login));
            }

            if (resposta.Erros != null && resposta.Erros.Any())
            {
                foreach (var erro in resposta.Erros)
                {
                    ModelState.AddModelError(string.Empty, erro);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, resposta.Mensagem);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("senacflix_token");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }

        // Classe auxiliar para mapear a resposta especifica do endpoint de Login da API
        public class LoginRespostaApi
        {
            public string Token { get; set; } = string.Empty;
            public System.DateTime Expiracao { get; set; }
            public string NomeUsuario { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string? FotoPerfilUrl { get; set; }
            public System.Collections.Generic.List<string> Perfis { get; set; } = new System.Collections.Generic.List<string>();
        }
    }
}
