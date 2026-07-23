using System.Threading.Tasks;
using SenacFlix.Domain.Entidades;

namespace SenacFlix.Domain.Interfaces
{
    public interface IEstatisticasRepositorio
    {
        Task<EstatisticasDashboard> ObterEstatisticasDashboardAsync();
    }
}