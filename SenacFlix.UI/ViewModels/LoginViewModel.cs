// Nome do arquivo: LoginViewModel.cs
// Objetivo: ViewModel para a tela de Login
// Camada: UI

using System.ComponentModel.DataAnnotations;

namespace SenacFlix.UI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O E-mail e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Digite um e-mail valido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha e obrigatoria.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}
