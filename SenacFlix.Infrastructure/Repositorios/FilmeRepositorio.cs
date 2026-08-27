// Nome do arquivo: FilmeRepositorio.cs
// Objetivo: Repositorio para acesso aos dados de Filme
// Camada: Infrastructure
// Como participa: Interage com SenacFlixContexto para realizar operacoes de CRUD em Filmes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            IQueryable<Filme> query = _contexto.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.ClassificacaoIndicativa);

            if (!incluirInativos)
            {
                query = query.Where(f => f.Ativo);
            }

            return await query.ToListAsync();
        }

        public async Task<Filme?> ObterPorIdAsync(int id)
        {
            return await _contexto.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.ClassificacaoIndicativa)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Filme>> BuscarAsync(string? termo, int? categoriaId = null)
        {
            var query = _contexto.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.ClassificacaoIndicativa)
                .Where(f => f.Ativo);

            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                query = query.Where(f => f.CategoriaId == categoriaId.Value);
            }

            if (!string.IsNullOrWhiteSpace(termo))
            {
                // Para ignorar case e acentos (dependendo do Collation do DB, o Like já resolve isso nativamente)
                var t = $"%{termo}%";
                query = query.Where(f =>
                    EF.Functions.Like(f.Titulo, t) ||
                    EF.Functions.Like(f.Descricao, t) ||
                    (f.Diretor != null && EF.Functions.Like(f.Diretor, t)) ||
                    (f.Elenco != null && EF.Functions.Like(f.Elenco, t)) ||
                    EF.Functions.Like(f.Categoria.Nome, t)
                );
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Filme>> ObterPorCategoriaAsync(int categoriaId)
        {
            return await _contexto.Filmes
                .Include(f => f.Categoria)
                .Include(f => f.ClassificacaoIndicativa)
                .Where(f => f.CategoriaId == categoriaId && f.Ativo)
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
