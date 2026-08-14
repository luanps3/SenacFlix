using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.Servicos.Interfaces;

namespace SenacFlix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        //Serviços Injetados
        private readonly IAuditoriaServico _auditoriaServico;

        public AuditoriaController(IAuditoriaServico auditoriaServico)
        {
            _auditoriaServico = auditoriaServico;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var resposta = await _auditoriaServico.ObterTodasAsync();
            return Ok(resposta);
        }

    }
}
