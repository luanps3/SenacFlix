// ============================================================
// Nome:         Favorito.cs
// Objetivo:     Representa o relacionamento entre um usuario e um filme
//               que ele marcou como favorito na plataforma SenacFlix.
//               Funciona como tabela de juncao (N:N) entre Usuario e Filme.
// Camada:       Domain (Entidades)
// Participa em: Relacionamento N:N entre ApplicationUser e Filme.
//               Permite ao usuario montar e consultar sua lista de favoritos.
// ============================================================

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade de juncao que registra quais filmes um usuario favoritou.
    /// Cada registro representa um favorito: um usuario + um filme + a data da acao.
    /// </summary>
    public class Favorito // Classe POCO que mapeia a tabela de favoritos
    {
        // --------------------------------------------------------
        // Chave primaria
        // --------------------------------------------------------

        /// <summary>
        /// Identificador unico do registro de favorito.
        /// Gerado automaticamente pelo banco de dados.
        /// </summary>
        public int Id { get; set; } // Chave primaria surrogada (autoincremento); alternativa a chave composta UsuarioId+FilmeId

        // --------------------------------------------------------
        // Relacionamento com o Usuario (ApplicationUser)
        // --------------------------------------------------------

        /// <summary>
        /// Identificador do usuario que favoritou o filme.
        /// E uma string pois o IdentityUser usa Guid (string) como chave primaria por padrao.
        /// Chave estrangeira para a tabela AspNetUsers (gerenciada pelo Identity).
        /// </summary>
        public required string UsuarioId { get; set; } // FK string pois IdentityUser.Id e do tipo string (GUID)

        /// <summary>
        /// Objeto de navegacao para o usuario que realizou o favorito.
        /// Preenchido pelo EF Core quando a consulta inclui o relacionamento.
        /// </summary>
        public ApplicationUser Usuario { get; set; } = null!; // null! pois o EF Core preenche em consultas com Include

        // --------------------------------------------------------
        // Relacionamento com o Filme
        // --------------------------------------------------------

        /// <summary>
        /// Identificador do filme que foi favoritado pelo usuario.
        /// Chave estrangeira para a tabela Filmes.
        /// </summary>
        public int FilmeId { get; set; } // FK inteira referenciando a tabela de filmes

        /// <summary>
        /// Objeto de navegacao para o filme favoritado.
        /// Preenchido pelo EF Core quando a consulta inclui o relacionamento.
        /// </summary>
        public Filme Filme { get; set; } = null!; // null! pois o EF Core cuida do preenchimento em contexto de consulta com Include

        // --------------------------------------------------------
        // Dados do registro de favorito
        // --------------------------------------------------------

        /// <summary>
        /// Data e hora exatas em que o usuario favoritou o filme.
        /// Util para ordenar a lista de favoritos por data de adicao.
        /// </summary>
        public DateTime DataFavorito { get; set; } // Registra o instante em que o favorito foi adicionado
    }
}
