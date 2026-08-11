// Nome do arquivo: ListaFilmesViewModel.cs
// Objetivo: ViewModel para exibir a tela de catalogo (com filmes, categorias para filtro e o filtro atual)
// Camada: UI

using System.Collections.Generic;

namespace SenacFlix.UI.ViewModels
{
    public class ListaFilmesViewModel
    {
        public List<FilmeViewModel> Filmes { get; set; } = new List<FilmeViewModel>();
        public List<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
        
        public FilmeViewModel? FilmeDestaque { get; set; }

        // Estado atual dos filtros na UI
        public int? CategoriaFiltro { get; set; }
        public string? TermoBusca { get; set; }
    }
}
