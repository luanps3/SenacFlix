// Nome do arquivo: FavoritoRepositorio.cs
// Objetivo: Repositorio para favoritos
// Camada: Infrastructure
// Como participa: Interage com o banco para consultar e adicionar favoritos do usuario

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class FavoritoRepositorio : IFavoritoRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public FavoritoRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Favorito>> ObterPorUsuarioAsync(string usuarioId)
        {
            return await _contexto.Favoritos
                .Include(f => f.Filme)
                    .ThenInclude(filme => filme.Categoria)
                .Where(f => f.UsuarioId == usuarioId)
                .OrderByDescending(f => f.DataFavorito)
                .ToListAsync();
        }

        public async Task<Favorito?> ObterAsync(string usuarioId, int filmeId)
        {
            return await _contexto.Favoritos
                .Include(f => f.Filme)
                .FirstOrDefaultAsync(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);
        }

        public async Task<Favorito> AdicionarAsync(Favorito favorito)
        {
            await _contexto.Favoritos.AddAsync(favorito);
            await _contexto.SaveChangesAsync();
            return favorito;
        }

        public async Task RemoverAsync(Favorito favorito)
        {
            _contexto.Favoritos.Remove(favorito);
            await _contexto.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(string usuarioId, int filmeId)
        {
            return await _contexto.Favoritos
                .AnyAsync(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);
        }
    }
}
