// Nome do arquivo: ClassificacaoViewModel.cs
// Objetivo: ViewModel fortemente tipado para Classificacao Indicativa no painel UI
// Camada: UI
// Como participa: Utilizado pelo FilmesController (Admin) para popular o dropdown de classificacoes,
//                 substituindo o uso incorreto de dynamic/JsonElement ao consumir a API.

namespace SenacFlix.UI.ViewModels
{
    /// <summary>
    /// ViewModel de leitura para a entidade ClassificacaoIndicativa.
    /// Espelha o ClassificacaoDto retornado pela API (/api/Classificacoes).
    /// Propriedades com PascalCase para compatibilidade com desserializacao JSON por padrao.
    /// </summary>
    public class ClassificacaoViewModel
    {
        // Identificador unico da classificacao
        public int Id { get; set; }

        // Nome da classificacao (ex: "Livre", "10 anos", "12 anos", "14 anos", "16 anos", "18 anos")
        public string Nome { get; set; } = string.Empty;

        // Idade minima recomendada (ex: 0, 10, 12, 14, 16, 18)
        public int IdadeMinima { get; set; }

        // Descricao detalhada dos criterios da classificacao
        public string? Descricao { get; set; }

        // Cor hexadecimal para exibicao do badge (ex: "#00AA00" para Livre)
        public string Cor { get; set; } = string.Empty;
    }
}
