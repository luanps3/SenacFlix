// Nome do arquivo: AuditoriaRepositorio.cs
// Objetivo: Repositorio de auditoria
// Camada: Infrastructure
// Como participa: Grava e recupera logs

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class AuditoriaRepositorio : IAuditoriaRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public AuditoriaRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Auditoria>> ObterTodasAsync()
        {
            return await _contexto.Auditorias
                .OrderByDescending(a => a.DataHora)
                .ToListAsync();
        }

        public async Task RegistrarAsync(Auditoria auditoria)
        {
            await _contexto.Auditorias.AddAsync(auditoria);
            await _contexto.SaveChangesAsync();
        }
    }
}
