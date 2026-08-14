using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SenacFlix.UI.ViewModels
{
    public class PerfilViewModel
    {
        public DadosPessoaisViewModel DadosPessoais { get; set; } = new DadosPessoaisViewModel();
        public AlterarSenhaViewModel AlterarSenha { get; set; } = new AlterarSenhaViewModel();
        public UploadFotoViewModel UploadFoto { get; set; } = new UploadFotoViewModel();
        
        // Info Somente Leitura
        public string Cargo { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public string StatusConta { get; set; } = string.Empty;
        public string? FotoAtualUrl { get; set; }
    }

    public class DadosPessoaisViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Sobrenome é obrigatório.")]
        public string Sobrenome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; } = string.Empty;

        public string? Telefone { get; set; }
    }

    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "A Senha Atual é obrigatória.")]
        [DataType(DataType.Password)]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Nova Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação da nova senha é obrigatória.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        [DataType(DataType.Password)]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }

    public class UploadFotoViewModel
    {
        [Required(ErrorMessage = "Selecione uma imagem para enviar.")]
        public IFormFile? NovaFoto { get; set; }
    }
}
