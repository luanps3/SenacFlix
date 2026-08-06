using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class EstatisticasRepositorio : IEstatisticasRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public EstatisticasRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<EstatisticasDashboard> ObterEstatisticasDashboardAsync()
        {
            var stats = new EstatisticasDashboard();

            // Totais
            stats.TotalFilmes = await _contexto.Filmes.CountAsync();
            stats.FilmesAtivos = await _contexto.Filmes.CountAsync(f => f.Ativo);
            stats.FilmesDesativados = stats.TotalFilmes - stats.FilmesAtivos;
            stats.TotalCategorias = await _contexto.Categorias.CountAsync();
            stats.TotalFavoritos = await _contexto.Favoritos.CountAsync();

            // Usuarios
            var roles = await _contexto.Roles.ToListAsync();
            var adminRoleId = roles.FirstOrDefault(r => r.Name == "Admin")?.Id;
            var operadorRoleId = roles.FirstOrDefault(r => r.Name == "Operador")?.Id;
            var clienteRoleId = roles.FirstOrDefault(r => r.Name == "Cliente")?.Id;

            var userRoles = await _contexto.UserRoles.ToListAsync();
            stats.TotalUsuarios = await _contexto.Users.CountAsync();
            stats.TotalAdministradores = userRoles.Count(ur => ur.RoleId == adminRoleId);
            stats.TotalOperadores = userRoles.Count(ur => ur.RoleId == operadorRoleId);
            stats.TotalClientes = userRoles.Count(ur => ur.RoleId == clienteRoleId);

            // Graficos
            stats.FilmesPorCategoria = await _contexto.Filmes
                .GroupBy(f => f.Categoria.Nome)
                .Select(g => new GraficoItem { Label = g.Key, Valor = g.Count() })
                .ToListAsync();

            stats.FilmesPorClassificacao = await _contexto.Filmes
                .GroupBy(f => f.ClassificacaoIndicativa.Nome)
                .Select(g => new GraficoItem { Label = g.Key, Valor = g.Count() })
                .ToListAsync();

            stats.FilmesPorAno = await _contexto.Filmes
                .GroupBy(f => f.AnoLancamento)
                .Select(g => new GraficoItem { Label = g.Key.ToString(), Valor = g.Count() })
                .OrderBy(g => g.Label)
                .ToListAsync();

            stats.UsuariosPorPerfil = new System.Collections.Generic.List<GraficoItem>
            {
                new GraficoItem { Label = "Administradores", Valor = stats.TotalAdministradores },
                new GraficoItem { Label = "Operadores", Valor = stats.TotalOperadores },
                new GraficoItem { Label = "Clientes", Valor = stats.TotalClientes }
            };

            stats.FavoritosPorCategoria = await _contexto.Favoritos
                .GroupBy(f => f.Filme.Categoria.Nome)
                .Select(g => new GraficoItem { Label = g.Key, Valor = g.Count() })
                .ToListAsync();

            stats.Top10FilmesFavoritados = await _contexto.Favoritos
                .GroupBy(f => f.Filme.Titulo)
                .Select(g => new GraficoItem { Label = g.Key, Valor = g.Count() })
                .OrderByDescending(g => g.Valor)
                .Take(10)
                .ToListAsync();

            return stats;
        }
    }
}