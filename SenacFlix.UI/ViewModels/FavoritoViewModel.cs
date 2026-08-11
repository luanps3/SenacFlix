// Nome do arquivo: FavoritoViewModel.cs
// Objetivo: ViewModel fortemente tipado para exibir os favoritos do usuario na View MVC
// Camada: UI
// Como participa: Recebe os dados desserializados da API (FavoritoDto) e os expoe para a View Index.cshtml

using System;

namespace SenacFlix.UI.ViewModels
{
    /// <summary>
    /// ViewModel que representa um favorito do usuario para exibicao na tela "Meus Favoritos".
    /// Espelha o FavoritoDto retornado pela API e adiciona propriedades calculadas uteis para a View.
    /// </summary>
    public class FavoritoViewModel
    {
        /// <summary>Id do registro de favorito.</summary>
        public int Id { get; set; }

        /// <summary>Id do usuario dono do favorito.</summary>
        public string UsuarioId { get; set; } = string.Empty;

        /// <summary>Id do filme favoritado.</summary>
        public int FilmeId { get; set; }

        /// <summary>Titulo do filme favoritado.</summary>
        public string FilmeTitulo { get; set; } = string.Empty;

        /// <summary>URL da imagem de capa do filme.</summary>
        public string? FilmeImagemCapaUrl { get; set; }

        /// <summary>Nome da categoria do filme.</summary>
        public string FilmeCategoriaNome { get; set; } = string.Empty;

        /// <summary>Data em que o usuario adicionou o filme aos favoritos.</summary>
        public DateTime DataFavorito { get; set; }
    }
}
