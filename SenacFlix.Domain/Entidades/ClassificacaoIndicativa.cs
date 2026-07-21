using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Domain.Entidades
{
    public class ClassificacaoIndicativa
    {
        public int Id { get; set; }

        [MaxLength(20,
            ErrorMessage 
            = "O nome da classificação deve ter no máximo 20 caracteres.")]
        public required string Nome { get; set; }
        public int IdadeMinima { get; set; }
        public string? Descricao { get; set; }
        public required string Cor { get; set; }
    }
}
