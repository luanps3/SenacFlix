// Nome do arquivo: CategoriaViewModel.cs
// Objetivo: ViewModel para categorias
// Camada: UI

namespace SenacFlix.UI.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int TotalFilmes { get; set; }
        public bool Ativo { get; set; }
    }
}
