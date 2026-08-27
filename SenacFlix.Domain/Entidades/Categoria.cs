// ============================================================
// Nome:         Categoria.cs
// Objetivo:     Representa uma categoria (genero/tema) pela qual os
//               filmes da plataforma SenacFlix sao organizados.
//               Exemplos: Acao, Comedia, Documentario, Drama.
// Camada:       Domain (Entidades)
// Participa em: Relacionamento com Filme (1 categoria para N filmes).
//               Usada na navegacao e filtragem do catalogo.
// ============================================================

using System.ComponentModel.DataAnnotations; // Importa atributos de validacao como [MaxLength] e [Required]

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade que representa uma categoria de filmes no SenacFlix.
    /// Cada filme pertence a uma unica categoria principal.
    /// </summary>
    public class Categoria // Classe POCO (Plain Old CLR Object) sem heranca especial
    {
        // --------------------------------------------------------
        // Chave primaria
        // --------------------------------------------------------

        /// <summary>
        /// Identificador unico da categoria no banco de dados.
        /// Gerado automaticamente pelo Entity Framework Core (identity).
        /// </summary>
        public int Id { get; set; } // Chave primaria da entidade; convencionalmente chamada "Id" para o EF Core reconhecer automaticamente

        // --------------------------------------------------------
        // Dados descritivos
        // --------------------------------------------------------

        /// <summary>
        /// Nome da categoria exibido na plataforma.
        /// Campo obrigatorio com no maximo 100 caracteres.
        /// Exemplos: "Acao", "Romance", "Terror", "Animacao".
        /// </summary>
        [MaxLength(100, ErrorMessage = "O nome da categoria deve ter no maximo 100 caracteres.")] // Limita o tamanho no banco e na validacao
        public required string Nome { get; set; } // required: obrigatorio na criacao do objeto em C# 11+

        /// <summary>
        /// Descricao opcional sobre o tipo de filmes desta categoria.
        /// Pode ser exibida como tooltip ou texto informativo na interface.
        /// </summary>
        public string? Descricao { get; set; } // Nullable: a descricao e opcional

        // --------------------------------------------------------
        // Controle de estado (soft delete e auditoria simples)
        // --------------------------------------------------------

        /// <summary>
        /// Indica se a categoria esta ativa e deve aparecer na plataforma.
        /// false = categoria desativada (exclusao logica).
        /// </summary>
        public bool Ativo { get; set; } = true; // Padrao true: toda categoria criada e ativa imediatamente

        /// <summary>
        /// Data e hora do cadastro da categoria no sistema.
        /// </summary>
        public DateTime DataCadastro { get; set; } // Registra quando a categoria foi criada

        /// <summary>
        /// Data e hora da ultima atualizacao da categoria.
        /// Null se nunca foi editada apos o cadastro.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; } // Nullable: preenchido somente em caso de edicao

        /// <summary>
        /// Data e hora da exclusao logica da categoria.
        /// Null enquanto a categoria nao for removida.
        /// </summary>
        public DateTime? DataExclusao { get; set; } // Nullable: preenchido somente ao desativar/excluir

        // --------------------------------------------------------
        // Navegacao (relacionamentos)
        // --------------------------------------------------------

        /// <summary>
        /// Colecao de filmes pertencentes a esta categoria.
        /// Representa o lado "um" do relacionamento 1:N (uma categoria, N filmes).
        /// </summary>
        public ICollection<Filme> Filmes { get; set; } = new List<Filme>(); // Inicializado como lista vazia para evitar NullReferenceException
    }
}
