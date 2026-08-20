using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Application.DTOs;
using SenacFlix.Domain.Entidades;

namespace SenacFlix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var users = await _userManager.Users.ToListAsync();
            var dtos = new List<UsuarioDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                dtos.Add(new UsuarioDto
                {
                    Id = user.Id,
                    NomeCompleto = user.NomeCompleto,
                    Email = user.Email!,
                    DataNascimento = user.DataNascimento,
                    FotoPerfilUrl = user.FotoPerfilUrl,
                    Ativo = user.Ativo,
                    DataCadastro = user.DataCadastro,
                    Perfis = roles.ToList()
                });
            }
            return Ok(ApiResposta<IEnumerable<UsuarioDto>>.Ok(dtos));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(ApiResposta<bool>.Falha("Usuario nao encontrado."));

            user.Ativo = false;
            await _userManager.UpdateAsync(user);

            return Ok(ApiResposta<bool>.Ok(true, "Usuario desativado com sucesso."));
        }

        [HttpPut("{id}/ativar")]
        public async Task<IActionResult> Ativar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound(ApiResposta<bool>.Falha("Usuario nao encontrado."));

            user.Ativo = true;
            await _userManager.UpdateAsync(user);

            return Ok(ApiResposta<bool>.Ok(true, "Usuario ativado com sucesso."));
        }


    }
}
