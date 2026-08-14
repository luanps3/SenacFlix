using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SenacFlix.Application.Servicos.Interfaces;

namespace SenacFlix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstatisticasController : ControllerBase
    {
        private readonly IEstatisticasServico _estatisticasServico;

        public EstatisticasController(IEstatisticasServico estatisticasServico)
        {
            _estatisticasServico = estatisticasServico;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> ObterEstatisticasDashboard()
        {
            var resposta = await _estatisticasServico.ObterEstatisticasDashboardAsync();
            return Ok(resposta);
        }
    }
}
