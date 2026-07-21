using System.ComponentModel.DataAnnotations;

namespace SenacFlix.Domain.Entidades
{
    public class Categoria
    {
        public int Id { get; set; }

        [MaxLength(100, 
            ErrorMessage = 
            "O nome da categoria deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataExclusao { get; set; }

        public ICollection<Filme> Filmes { get; set; } = new List<Filme>();
    }
}
