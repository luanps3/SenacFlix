// Nome do arquivo: FilmeViewModel.cs
// Objetivo: ViewModel para exibir dados do filme nas Views MVC
// Camada: UI

namespace SenacFlix.UI.ViewModels
{
    public class FilmeViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        // Duracao armazenada em minutos (ex: 169). Use DuracaoFormatada para exibicao nas Views.
        public int Duracao { get; set; }
        public string? Diretor { get; set; }
        public string? Elenco { get; set; }
        public string? ImagemCapaUrl { get; set; }
        public string? ImagemBannerUrl { get; set; }
        public string? TrailerYoutubeUrl { get; set; }
        public string? VideoYoutubeUrl { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;
        public int ClassificacaoIndicativaId { get; set; }
        public string ClassificacaoNome { get; set; } = string.Empty;
        public string ClassificacaoCor { get; set; } = string.Empty;
        public int ClassificacaoIdadeMinima { get; set; }
        public bool Ativo { get; set; }
        public bool DestaqueHome { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }
        
        // Propriedade usada na UI para saber se o usuario atual favoritou este filme
        public bool EhFavorito { get; set; }

        // Propriedade calculada que converte Duracao (minutos) para formato legivel na View
        // Exemplo: 169 -> "2h 49min" | 90 -> "1h 30min" | 45 -> "45min"
        public string DuracaoFormatada
        {
            get
            {
                if (Duracao <= 0) return "Desconhecida";
                int horas = Duracao / 60;
                int mins = Duracao % 60;
                if (horas > 0 && mins > 0) return $"{horas}h {mins}min";
                if (horas > 0) return $"{horas}h";
                return $"{mins}min";
            }
        }

        // Propriedade calculada para transformar URL normal do YouTube em URL de Embed
        public string UrlEmbedTrailer 
        {
            get
            {
                if (string.IsNullOrEmpty(TrailerYoutubeUrl)) return string.Empty;
                return ConverterParaEmbed(TrailerYoutubeUrl);
            }
        }

        public string UrlEmbedVideo
        {
            get
            {
                if (string.IsNullOrEmpty(VideoYoutubeUrl)) return string.Empty;
                return ConverterParaEmbed(VideoYoutubeUrl);
            }
        }

        private string ConverterParaEmbed(string url)
        {
            // Exemplo simples de conversao de URL youtube
            // Transforma "https://www.youtube.com/watch?v=12345" em "https://www.youtube.com/embed/12345"
            if (url.Contains("watch?v="))
            {
                return url.Replace("watch?v=", "embed/");
            }
            if (url.Contains("youtu.be/"))
            {
                return url.Replace("youtu.be/", "youtube.com/embed/");
            }
            return url;
        }
    }
}
