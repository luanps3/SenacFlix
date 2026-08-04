using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class FilmeRepositorio : IFilmeRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public FilmeRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Filme>> ObterTodosAsync(bool incluirInativos = false)
        {
            // IQueryable representa uma consulta que pode ser executada em uma fonte de dados.
            // Ele permite construir consultas de forma flexível e eficiente,
            // sem executar a consulta imediatamente.
           
            IQueryable<Filme> query = _contexto.Filmes
                .Include(filme => filme.Categoria)
                .Include(filme => filme.ClassificacaoIndicativa);

            // Se incluirInativos for false, filtra apenas os filmes ativos
            if (!incluirInativos)
            {
                query = query.Where(filme => filme.Ativo);
            }

            return await query.ToListAsync();
        }

        public async Task<Filme?> ObterPorIdAsync(int id)
        {
            return await _contexto.Filmes
                .Include(filme => filme.Categoria)
                .Include(filme => filme.ClassificacaoIndicativa)
                .FirstOrDefaultAsync(filme => filme.Id == id);
        }

        public async Task<IEnumerable<Filme>> BuscarAsync(string? termo, int? categoriaId = null)
        {
            var query = _contexto.Filmes
                .Include(filme => filme.Categoria)
                .Include(filme => filme.ClassificacaoIndicativa)
                .Where(filme => filme.Ativo);

            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                query = query.Where(filme => filme.CategoriaId == categoriaId.Value);
            }

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var t = $"%{termo}%";
                // EF.Functions.Like é usado para realizar uma comparação de padrão SQL,
                // permitindo o uso de curingas como %
                query = query.Where(filme => 
                EF.Functions.Like(filme.Titulo, t) ||
                EF.Functions.Like(filme.Descricao, t) ||
                (filme.Diretor != null && EF.Functions.Like(filme.Diretor, t)) ||
                EF.Functions.Like(filme.Categoria.Nome, t)
                );
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Filme>> ObterPorCategoriaAsync(int categoriaId)
        {
            return await _contexto.Filmes
                .Include(filme => filme.Categoria)
                .Include(filme => filme.ClassificacaoIndicativa)
                .Where(filme => filme.CategoriaId == categoriaId && filme.Ativo)
                .ToListAsync();
        }

        public async Task<Filme> AdicionarAsync(Filme filme)
        {
            await _contexto.Filmes.AddAsync(filme);
            await _contexto.SaveChangesAsync();
            return filme;
        }

        public async Task AtualizarAsync(Filme filme)
        {
            _contexto.Filmes.Update(filme);
            await _contexto.SaveChangesAsync();
        }

        public async Task DesativarAsync(int id)
        {
            var filme = await _contexto.Filmes.FindAsync(id);
            if (filme != null)
            {
                filme.Ativo = false;
                filme.DataExclusao = DateTime.UtcNow;
                filme.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task ExcluirPermanentementeAsync(int id)
        {
            var filme = await _contexto.Filmes.FindAsync(id);
            if (filme != null)
            {
                _contexto.Filmes.Remove(filme);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task ReativarAsync(int id)
        {
            var filme = await _contexto.Filmes.FindAsync(id);
            if (filme != null && !filme.Ativo)
            {
                filme.Ativo = true;
                filme.DataExclusao = null;
                filme.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }





    }
}
