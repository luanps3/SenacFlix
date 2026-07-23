// ============================================================
// Nome:         IClassificacaoRepositorio.cs
// Objetivo:     Define o contrato de acesso a dados para a entidade
//               ClassificacaoIndicativa no SenacFlix.
//               As classificacoes sao dados de referencia (lookup table)
//               raramente alterados; por isso o contrato e simples.
// Camada:       Domain (Interfaces)
// Participa em: Implementada pela camada Infrastructure.
//               Consumida na camada Application ao cadastrar/editar filmes,
//               para popular o selector de classificacao indicativa.
// ============================================================

using SenacFlix.Domain.Entidades; // Importa a entidade ClassificacaoIndicativa

namespace SenacFlix.Domain.Interfaces // Define o namespace da camada de dominio, pasta Interfaces
{
    /// <summary>
    /// Contrato que define as operacoes de leitura para a entidade ClassificacaoIndicativa.
    /// Por ser uma tabela de referencia (Livre, 10+, 12+, 14+, 16+, 18+),
    /// as operacoes de escrita sao realizadas apenas via migrations ou seed inicial,
    /// portanto o contrato expoe apenas metodos de consulta.
    /// </summary>
    public interface IClassificacaoRepositorio // interface: contrato de acesso somente-leitura para tabela de referencia
    {
        /// <summary>
        /// Retorna todas as classificacoes indicativas cadastradas no sistema.
        /// Usado para popular selects e filtros na interface da plataforma.
        /// O resultado e pequeno e pode ser cacheado na camada de aplicacao.
        /// </summary>
        /// <returns>Colecao enumeravel de todas as classificacoes indicativas.</returns>
        Task<IEnumerable<ClassificacaoIndicativa>> ObterTodasAsync(); // Tabela de referencia pequena; retorna todos os registros sem filtro

        /// <summary>
        /// Retorna uma classificacao indicativa especifica pelo seu identificador.
        /// Usado para validar se uma classificacao informada no cadastro de filme e valida.
        /// Retorna null se o identificador nao existir no banco de dados.
        /// </summary>
        /// <param name="id">Identificador unico da classificacao indicativa.</param>
        /// <returns>A classificacao encontrada ou null.</returns>
        Task<ClassificacaoIndicativa?> ObterPorIdAsync(int id); // ClassificacaoIndicativa? (nullable): retorna null se id invalido
    }
}
