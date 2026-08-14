using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.Servicos.Interfaces;

namespace SenacFlix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificacoesController : ControllerBase
    {
        //Serviços Injetados
        private readonly IClassificacaoServico _classificacaoServico;

        public ClassificacoesController(IClassificacaoServico classificacaoServico)
        {
            _classificacaoServico = classificacaoServico;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var resposta = await _classificacaoServico.ObterTodasAsync();
            return Ok(resposta);
        }

    }
}
