// Nome do arquivo: ClassificacaoRepositorio.cs
// Objetivo: Repositorio de classificacao indicativa
// Camada: Infrastructure
// Como participa: Consulta a tabela de classificacoes

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class ClassificacaoRepositorio : IClassificacaoRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public ClassificacaoRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<ClassificacaoIndicativa>> ObterTodasAsync()
        {
            return await _contexto.ClassificacoesIndicativas
                .OrderBy(c => c.IdadeMinima)
                .ToListAsync();
        }

        public async Task<ClassificacaoIndicativa?> ObterPorIdAsync(int id)
        {
            return await _contexto.ClassificacoesIndicativas
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
