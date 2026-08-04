
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class ClassificacaoRepositorio : IClassificacaoRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        // Injeção de depêndencia do Contexto +
        // Encapsulamento do contexto em um repositório para acesso a dados.
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
            return await _contexto.ClassificacoesIndicativas.FirstOrDefaultAsync(classificacao => classificacao.Id == id);
        } 
    }
}
