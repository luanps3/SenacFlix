// Nome do arquivo: DashboardEstatisticasViewModel.cs
// Objetivo: ViewModel para receber e exibir os dados do Dashboard Administrativo
// Camada: UI
// Como participa: Preenchido pelo DashboardController apos chamar a API e passado para a View.

using System.Collections.Generic;

namespace SenacFlix.UI.ViewModels
{
    // ViewModel principal do Dashboard
    public class DashboardEstatisticasViewModel
    {
        // Contadores gerais
        public int TotalFilmes { get; set; }
        public int FilmesAtivos { get; set; }
        public int FilmesDesativados { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalAdministradores { get; set; }
        public int TotalOperadores { get; set; }
        public int TotalClientes { get; set; }
        public int TotalFavoritos { get; set; }

        // Dados para os graficos
        public List<GraficoItemViewModel> FilmesPorCategoria { get; set; } = new();
        public List<GraficoItemViewModel> FilmesPorClassificacao { get; set; } = new();
        public List<GraficoItemViewModel> FilmesPorAno { get; set; } = new();
        public List<GraficoItemViewModel> UsuariosPorPerfil { get; set; } = new();
        public List<GraficoItemViewModel> FavoritosPorCategoria { get; set; } = new();
        public List<GraficoItemViewModel> Top10FilmesFavoritados { get; set; } = new();
    }

    // Item individual de grafico (label + valor numerico)
    public class GraficoItemViewModel
    {
        public string Label { get; set; } = string.Empty;
        public int Valor { get; set; }
    }
}
