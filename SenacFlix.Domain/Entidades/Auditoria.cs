// ============================================================
// Nome:         Auditoria.cs
// Objetivo:     Registra um log de acoes realizadas no sistema SenacFlix,
//               permitindo rastrear quem fez o que e quando.
//               Essencial para conformidade, seguranca e depuracao.
// Camada:       Domain (Entidades)
// Participa em: Gravacao automatica de eventos criticos (CRUD, login, logout).
//               Consultada por administradores para monitorar a plataforma.
// ============================================================

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade que armazena registros de auditoria das acoes realizadas no sistema.
    /// Cada linha representa um evento: criacao, atualizacao, exclusao, login ou logout.
    /// Os registros de auditoria NUNCA sao excluidos; servem como trilha de auditoria.
    /// </summary>
    public class Auditoria // Classe de entidade de auditoria; nao possui exclusao logica intencional
    {
        // --------------------------------------------------------
        // Chave primaria
        // --------------------------------------------------------

        /// <summary>
        /// Identificador unico do registro de auditoria.
        /// Gerado automaticamente pelo banco de dados.
        /// </summary>
        public int Id { get; set; } // Chave primaria autoincremento; convenção do EF Core

        // --------------------------------------------------------
        // Informacoes do ator (quem realizou a acao)
        // --------------------------------------------------------

        /// <summary>
        /// Identificador do usuario que realizou a acao auditada.
        /// Pode ser nulo para acoes do sistema (jobs automatizados, migrações).
        /// E uma string pois o IdentityUser usa string (GUID) como chave primaria.
        /// </summary>
        public string? UsuarioId { get; set; } // Nullable: acoes do sistema podem nao ter usuario associado

        /// <summary>
        /// Nome de exibicao ou e-mail do usuario no momento da acao.
        /// Armazenado diretamente pois o usuario pode ser excluido no futuro,
        /// e o registro de auditoria precisa preservar essa informacao historica.
        /// </summary>
        public string? NomeUsuario { get; set; } // Nullable e desnormalizado intencionalmente para preservar o historico

        // --------------------------------------------------------
        // Dados da acao auditada
        // --------------------------------------------------------

        /// <summary>
        /// Descricao da acao realizada.
        /// Pode conter o nome do TipoAcao convertido em string.
        /// Exemplos: "Criacao", "Atualizacao", "Login", "ExclusaoLogica".
        /// </summary>
        public required string Acao { get; set; } // required: toda auditoria precisa identificar qual foi a acao

        /// <summary>
        /// Nome da tabela ou entidade afetada pela acao.
        /// Exemplos: "Filmes", "Categorias", "AspNetUsers".
        /// </summary>
        public required string TabelaAfetada { get; set; } // required: necessario saber em qual recurso a acao ocorreu

        /// <summary>
        /// Detalhes adicionais sobre a acao, como os dados alterados (JSON ou texto livre).
        /// Exemplos: valores anteriores/posteriores em uma atualizacao, IP de origem.
        /// </summary>
        public string? Detalhes { get; set; } // Nullable: campo livre para informacoes complementares da acao

        // --------------------------------------------------------
        // Metadado temporal
        // --------------------------------------------------------

        /// <summary>
        /// Data e hora exatas em que a acao foi registrada no sistema.
        /// Armazenado em UTC para consistencia em ambientes distribuidos.
        /// </summary>
        public DateTime DataHora { get; set; } // Timestamp do evento; idealmente em UTC (DateTime.UtcNow)
    }
}
