using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.UI.Servicos;
using SenacFlix.UI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SenacFlix.UI.Controllers
{
    public class AtualizarPerfilDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
    }

    public class AlterarSenhaDto
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }

    public class UsuarioDto
    {
        public string Id { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public List<string> Perfis { get; set; } = new List<string>();
    }

    [Authorize]
    public class PerfilController : Controller
    {
        private readonly ApiCliente _api;

        public PerfilController(ApiCliente api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = await CarregarPerfilViewModelAsync();
            if (vm == null) return RedirectToAction("Login", "Conta");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarDadosPessoais(PerfilViewModel model)
        {
            var dto = new AtualizarPerfilDto
            {
                Nome = model.DadosPessoais.Nome,
                Sobrenome = model.DadosPessoais.Sobrenome,
                Email = model.DadosPessoais.Email,
                Telefone = model.DadosPessoais.Telefone
            };

            var resposta = await _api.PutAsync<SenacFlix.UI.Servicos.ApiResposta<object>, AtualizarPerfilDto>("/api/perfil", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Dados pessoais atualizados com sucesso!";
                await AtualizarClaimsUsuarioAsync(dto.Nome + " " + dto.Sobrenome, null);
            }
            else
            {
                TempData["Erro"] = resposta.Mensagem ?? "Erro ao atualizar dados.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AlterarSenha(PerfilViewModel model)
        {
            var dto = new AlterarSenhaDto
            {
                SenhaAtual = model.AlterarSenha.SenhaAtual,
                NovaSenha = model.AlterarSenha.NovaSenha,
                ConfirmarNovaSenha = model.AlterarSenha.ConfirmarNovaSenha
            };

            var resposta = await _api.PutAsync<SenacFlix.UI.Servicos.ApiResposta<object>, AlterarSenhaDto>("/api/perfil/senha", dto);

            if (resposta.Sucesso)
            {
                TempData["Sucesso"] = "Senha alterada com sucesso!";
            }
            else
            {
                TempData["Erro"] = resposta.Mensagem ?? "Erro ao alterar senha.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadFoto(PerfilViewModel model)
        {
            if (model.UploadFoto.NovaFoto == null)
            {
                TempData["Erro"] = "Nenhuma imagem foi selecionada.";
                return RedirectToAction(nameof(Index));
            }

            var formData = new System.Net.Http.MultipartFormDataContent();
            var streamContent = new System.Net.Http.StreamContent(model.UploadFoto.NovaFoto.OpenReadStream());
            formData.Add(streamContent, "arquivo", model.UploadFoto.NovaFoto.FileName);

            var resposta = await _api.PostMultipartAsync<string>("/api/perfil/foto", formData);

            if (resposta.Sucesso && !string.IsNullOrEmpty(resposta.Dados))
            {
                TempData["Sucesso"] = "Foto de perfil atualizada!";
                await AtualizarClaimsUsuarioAsync(null, resposta.Dados);
            }
            else
            {
                TempData["Erro"] = resposta.Mensagem ?? "Erro ao fazer upload da foto.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<PerfilViewModel?> CarregarPerfilViewModelAsync()
        {
            var resposta = await _api.GetAsync<UsuarioDto>("/api/perfil");
            if (!resposta.Sucesso || resposta.Dados == null) return null;

            var user = resposta.Dados;
            
            var partesNome = user.NomeCompleto.Split(' ');
            var nome = partesNome.Length > 0 ? partesNome[0] : "";
            var sobrenome = partesNome.Length > 1 ? string.Join(" ", partesNome.Skip(1)) : "";

            var vm = new PerfilViewModel
            {
                DadosPessoais = new DadosPessoaisViewModel
                {
                    Nome = nome,
                    Sobrenome = sobrenome,
                    Email = user.Email
                },
                Cargo = string.Join(", ", user.Perfis),
                DataCadastro = user.DataCadastro,
                StatusConta = user.Ativo ? "Ativo" : "Inativo",
                FotoAtualUrl = user.FotoPerfilUrl
            };

            return vm;
        }

        // Helper para atualizar o Nome e a Foto sem relogar
        private async Task AtualizarClaimsUsuarioAsync(string? novoNome, string? novaFotoUrl)
        {
            var identity = (ClaimsIdentity)User.Identity!;
            
            if (novoNome != null)
            {
                var claimNome = identity.FindFirst(ClaimTypes.Name);
                if (claimNome != null) identity.RemoveClaim(claimNome);
                identity.AddClaim(new Claim(ClaimTypes.Name, novoNome));
            }

            if (novaFotoUrl != null)
            {
                var claimFoto = identity.FindFirst("FotoPerfil");
                if (claimFoto != null) identity.RemoveClaim(claimFoto);
                identity.AddClaim(new Claim("FotoPerfil", novaFotoUrl));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
