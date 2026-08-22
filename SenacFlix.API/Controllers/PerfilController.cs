using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.DTOs;
using SenacFlix.Domain.Entidades;
using System.Security.Claims;

namespace SenacFlix.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PerfilController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPerfil()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.Ativo) return NotFound(ApiResposta<object>.Falha("Usuário não encontrado."));

            var roles = await _userManager.GetRolesAsync(user);

            var dto = new UsuarioDto
            {
                Id = user.Id,
                NomeCompleto = user.NomeCompleto,
                Email = user.Email!,
                DataNascimento = user.DataNascimento,
                FotoPerfilUrl = user.FotoPerfilUrl,
                Ativo = user.Ativo,
                DataCadastro = user.DataCadastro,
                Perfis = roles.ToList()
            };

            return Ok(ApiResposta<UsuarioDto>.Ok(dto));
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarPerfil([FromBody] AtualizarPerfilDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.Ativo) return NotFound(ApiResposta<object>.Falha("Usuário não encontrado."));

            // Concatenar Nome e Sobrenome para manter o padrão ApplicationUser.NomeCompleto
            user.NomeCompleto = $"{dto.Nome.Trim()} {dto.Sobrenome.Trim()}";

            // Alterar e-mail utilizando os recursos do Identity
            if (user.Email != dto.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(dto.Email);
                if (emailExists != null)
                {
                    return BadRequest(ApiResposta<object>.Falha("Este e-mail já está em uso por outra conta."));
                }
                user.Email = dto.Email;
                user.UserName = dto.Email;
            }

            user.PhoneNumber = dto.Telefone;
            user.DataAtualizacao = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResposta<object>.FalhaValidacao(erros, "Erro ao atualizar perfil."));
            }

            return Ok(ApiResposta<object>.Ok(null, "Perfil atualizado com sucesso."));
        }

        [HttpPut("senha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.Ativo) return NotFound(ApiResposta<object>.Falha("Usuário não encontrado."));

            var result = await _userManager.ChangePasswordAsync(user, dto.SenhaAtual, dto.NovaSenha);
            if (!result.Succeeded)
            {
                var erros = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResposta<object>.FalhaValidacao(erros, "Erro ao alterar senha. Verifique sua senha atual."));
            }
            return Ok(ApiResposta<object>.Ok(null, "Senha alterada com sucesso."));
        }

        [HttpPost("foto")]
        public async Task<IActionResult> UploadFoto(IFormFile arquivo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !user.Ativo) return NotFound(ApiResposta<object>.Falha("Usuário não encontrado."));

            if (arquivo == null || arquivo.Length == 0)
                return BadRequest(ApiResposta<object>.Falha("Arquivo não selecionado."));

            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var tamanhoMaximo = 5 * 1024 * 1024; // 5MB

            if (arquivo.Length > tamanhoMaximo)
                return BadRequest(ApiResposta<object>.Falha("O arquivo excede o tamanho máximo de 5MB."));

            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extensao) || !extensoesPermitidas.Contains(extensao))
                return BadRequest(ApiResposta<object>.Falha("Extensão de arquivo não permitida. Aceitos apenas JPG, PNG e WEBP."));

            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

            var pastaDestinoFisico = Path.Combine(Directory.GetCurrentDirectory(), "..", "SenacFlix.UI", "wwwroot", "uploads", "perfis");

            if (!Directory.Exists(pastaDestinoFisico))
                Directory.CreateDirectory(pastaDestinoFisico);

            var caminhoFisicoCompleto = Path.Combine(pastaDestinoFisico, nomeArquivo);

            using (var stream = new FileStream(caminhoFisicoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            if (!string.IsNullOrEmpty(user.FotoPerfilUrl) && !user.FotoPerfilUrl.StartsWith("http") && !user.FotoPerfilUrl.Contains("default"))
            {
                var oldFileName = Path.GetFileName(user.FotoPerfilUrl);
                var oldFilePath = Path.Combine(pastaDestinoFisico, oldFileName);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }
            var caminhoRelativo = $"/uploads/perfis/{nomeArquivo}";

            user.FotoPerfilUrl = caminhoRelativo;
            user.DataAtualizacao = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Ok(ApiResposta<string>.Ok(caminhoRelativo, "Foto de perfil atualizada com sucesso."));
        }
    }
}
