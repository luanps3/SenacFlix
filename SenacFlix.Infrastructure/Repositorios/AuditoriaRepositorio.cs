using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private readonly SenacFlixContexto _contexto;
    }
}
