// Nome do arquivo: CategoriasController.cs
// Objetivo: Controlador para categorias
// Camada: API
// Como participa: Recebe requisicoes HTTP para manipular categorias

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SenacFlix.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        //Serviços Injetados
        private readonly ICategoriaServico _categoriaServico;

        public CategoriasController(ICategoriaServico categoriaServico)
        {
            _categoriaServico = categoriaServico;
        }


        [HttpGet]
        public async Task<IActionResult> ObterAtivas()
        {
            var resposta = await _categoriaServico.ObterTodasAsync(incluirInativas: false);
            return Ok(resposta);
        }

        [HttpGet("todas")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> ObterTodas()
        {
            var resposta = await _categoriaServico.ObterTodasAsync(incluirInativas: true);
            return Ok(resposta);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resposta = await _categoriaServico.ObterPorIdAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Cadastrar([FromBody] CriarCategoriaDto dto)
        {
            var resposta = await _categoriaServico.CadastrarAsync(dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return StatusCode(201, resposta);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CriarCategoriaDto dto)
        {
            var resposta = await _categoriaServico.AtualizarAsync(id, dto);
            if (!resposta.Sucesso) return BadRequest(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/desativar")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Desativar(int id)
        {
            var resposta = await _categoriaServico.DesativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpPut("{id}/reativar")]
        [Authorize(Roles = "Admin,Operador")]
        public async Task<IActionResult> Reativar(int id)
        {
            var resposta = await _categoriaServico.ReativarAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }

        [HttpDelete("{id}/permanente")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExcluirPermanente(int id)
        {
            var resposta = await _categoriaServico.ExcluirPermanentementeAsync(id);
            if (!resposta.Sucesso) return NotFound(resposta);
            return Ok(resposta);
        }
    }
}
