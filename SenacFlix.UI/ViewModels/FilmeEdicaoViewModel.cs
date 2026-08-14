// Nome do arquivo: FilmeEdicaoViewModel.cs
// Objetivo: ViewModel para tela de edicao e criacao de Filmes no painel Admin
// Camada: UI

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.UI.ViewModels
{
    public class FilmeEdicaoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Titulo e obrigatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Descricao e obrigatoria")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Ano de Lancamento e obrigatorio")]
        [Display(Name = "Ano de Lançamento")]
        public int AnoLancamento { get; set; }

        [Required(ErrorMessage = "A Duracao e obrigatoria")]
        [Display(Name = "Duração (em minutos)")]
        public int Duracao { get; set; }

        public string? Diretor { get; set; }
        public string? Elenco { get; set; }

        // Propriedades para exibicao da imagem atual
        public string? ImagemCapaUrlAtual { get; set; }
        public string? ImagemBannerUrlAtual { get; set; }

        // Tipo de insercao: "Upload" ou "Url"
        public string TipoCapa { get; set; } = "Upload";
        public string TipoBanner { get; set; } = "Upload";

        // Propriedades para upload de novas imagens
        [Display(Name = "Nova Imagem de Capa (Upload)")]
        public IFormFile? NovaImagemCapa { get; set; }

        [Display(Name = "URL da Nova Imagem de Capa")]
        public string? NovaImagemCapaUrlInfo { get; set; }

        [Display(Name = "Nova Imagem de Banner (Upload)")]
        public IFormFile? NovaImagemBanner { get; set; }

        [Display(Name = "URL da Nova Imagem de Banner")]
        public string? NovaImagemBannerUrlInfo { get; set; }

        [Display(Name = "URL do Trailer no YouTube")]
        public string? TrailerYoutubeUrl { get; set; }

        [Display(Name = "URL do Video no YouTube")]
        public string? VideoYoutubeUrl { get; set; }

        [Required(ErrorMessage = "Selecione uma categoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Selecione a classificação indicativa")]
        [Display(Name = "Classificação Indicativa")]
        public int ClassificacaoIndicativaId { get; set; }

        // Listas para popular as dropdowns na view
        public IEnumerable<SelectListItem> CategoriasDisponiveis { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ClassificacoesDisponiveis { get; set; } = new List<SelectListItem>();
    }
}
