// Nome do arquivo: CategoriaRepositorio.cs
// Objetivo: Repositorio para acesso aos dados de Categoria
// Camada: Infrastructure
// Como participa: Interage com SenacFlixContexto para realizar operacoes de CRUD em Categorias

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
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public CategoriaRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Categoria>> ObterTodasAsync(bool incluirInativas = false)
        {
            IQueryable<Categoria> query = _contexto.Categorias
                .Include(c => c.Filmes.Where(f => f.Ativo));

            if (!incluirInativas)
            {
                query = query.Where(c => c.Ativo);
            }

            return await query.ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            return await _contexto.Categorias
                .Include(c => c.Filmes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Categoria> AdicionarAsync(Categoria categoria)
        {
            await _contexto.Categorias.AddAsync(categoria);
            await _contexto.SaveChangesAsync();
            return categoria;
        }

        public async Task AtualizarAsync(Categoria categoria)
        {
            _contexto.Categorias.Update(categoria);
            await _contexto.SaveChangesAsync();
        }

        public async Task DesativarAsync(int id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Ativo = false;
                categoria.DataExclusao = DateTime.UtcNow;
                categoria.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task ReativarAsync(int id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Ativo = true;
                categoria.DataExclusao = null;
                categoria.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task ExcluirPermanentementeAsync(int id)
        {
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _contexto.Categorias.Remove(categoria);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}
