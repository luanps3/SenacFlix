// Nome do arquivo: RegistroViewModel.cs
// Objetivo: ViewModel para tela de criacao de conta
// Camada: UI

using System;
using System.ComponentModel.DataAnnotations;

namespace SenacFlix.UI.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "O Nome Completo e obrigatorio.")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O E-mail e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail valido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha e obrigatoria.")]
        [StringLength(100, ErrorMessage = "A senha deve ter no minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmacao de senha nao coincidem.")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Data de Nascimento e obrigatoria.")]
        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNascimento { get; set; }
    }
}
