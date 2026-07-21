using Microsoft.AspNetCore.Identity;

namespace SenacFlix.Domain.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        public required string NomeCompleto { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }

    }
}
