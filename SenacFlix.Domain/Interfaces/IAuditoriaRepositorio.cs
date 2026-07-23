// ============================================================
// Nome:         IAuditoriaRepositorio.cs
// Objetivo:     Define o contrato de acesso a dados para a entidade Auditoria,
//               permitindo registrar e consultar o log de acoes do sistema
//               SenacFlix de forma desacoplada da implementacao de banco.
// Camada:       Domain (Interfaces)
// Participa em: Implementada pela camada Infrastructure.
//               Consumida pela camada Application para gravar eventos auditaveis.
//               Consumida por controllers administrativos para consultar historico.
// ============================================================

using SenacFlix.Domain.Entidades; // Importa a entidade Auditoria

namespace SenacFlix.Domain.Interfaces // Define o namespace da camada de dominio, pasta Interfaces
{
    /// <summary>
    /// Contrato que define as operacoes de acesso a dados para a entidade Auditoria.
    /// A auditoria e write-heavy: registros sao gravados com frequencia e raramente consultados.
    /// Registros de auditoria nunca sao excluidos ou atualizados apos a gravacao.
    /// </summary>
    public interface IAuditoriaRepositorio // interface: apenas assinaturas, sem logica
    {
        /// <summary>
        /// Retorna todos os registros de auditoria do sistema em ordem cronologica.
        /// Metodo usado por administradores para monitorar as acoes realizadas.
        /// Em producao, recomenda-se adicionar filtros de data e paginacao.
        /// </summary>
        /// <returns>Colecao enumeravel de todos os registros de auditoria.</returns>
        Task<IEnumerable<Auditoria>> ObterTodasAsync(); // Consulta geral; em producao deve ser paginada para evitar sobrecarga

        /// <summary>
        /// Grava um novo registro de auditoria no banco de dados.
        /// Deve ser chamado sempre que uma acao critica for realizada no sistema.
        /// Este metodo nao retorna dados; apenas persiste o evento.
        /// </summary>
        /// <param name="auditoria">Objeto Auditoria preenchido com os dados do evento.</param>
        Task RegistrarAsync(Auditoria auditoria); // Insercao simples; Task sem retorno pois nao e necessario o Id gerado
    }
}