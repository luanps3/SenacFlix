using System.Collections.Generic;

namespace SenacFlix.Domain.Entidades
{
    public class EstatisticasDashboard
    {
        public int TotalFilmes { get; set; }
        public int FilmesAtivos { get; set; }
        public int FilmesDesativados { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalAdministradores { get; set; }
        public int TotalOperadores { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFavoritos { get; set; }

        public List<GraficoItem> FilmesPorCategoria { get; set; } = new();
        public List<GraficoItem> FilmesPorClassificacao { get; set; } = new();
        public List<GraficoItem> FilmesPorAno { get; set; } = new();
        public List<GraficoItem> UsuariosPorPerfil { get; set; } = new();
        public List<GraficoItem> FavoritosPorCategoria { get; set; } = new();
        public List<GraficoItem> Top10FilmesFavoritados { get; set; } = new();
    }

    public class GraficoItem
    {
        public string Label { get; set; } = string.Empty;
        public int Valor { get; set; }
    }
}