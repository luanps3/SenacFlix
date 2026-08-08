// Nome do arquivo: CategoriasController.cs
// Objetivo: Controlador para categorias
// Camada: API
// Como participa: Recebe requisicoes HTTP para manipular categorias

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;

namespace SenacFlix.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaServico _servico;

        public CategoriasController(ICategoriaServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public async Task<IActionResult> ObterAtivas()
        {
            var resposta = await _servico.ObterTodasAsync(incluirInativas: false);
            return Ok(resposta);
        }

        [HttpGet("todas")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> ObterTodas()
        {
            var resposta = await _servico.ObterTodasAsync(incluirInativas: true);
            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resposta = await _servico.ObterPorIdAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Cadastrar([FromBody] CriarCategoriaDto dto)
        {
            var resposta = await _servico.CadastrarAsync(dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return StatusCode(201, resposta);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CriarCategoriaDto dto)
        {
            var resposta = await _servico.AtualizarAsync(id, dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/desativar")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _servico.DesativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPut("{id}/reativar")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _servico.ReativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/permanente")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirPermanente(int id)
        {
            var resposta = await _servico.ExcluirPermanentementeAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }
    }
}
