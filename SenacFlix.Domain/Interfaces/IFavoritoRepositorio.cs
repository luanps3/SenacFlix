// ============================================================
// Nome:         IFavoritoRepositorio.cs
// Objetivo:     Define o contrato de acesso a dados para a entidade Favorito,
//               permitindo gerenciar a lista de filmes favoritos de cada usuario
//               no SenacFlix de forma desacoplada da implementacao de banco.
// Camada:       Domain (Interfaces)
// Participa em: Implementada pela camada Infrastructure.
//               Consumida pela camada Application nos servicos de favoritos.
// ============================================================

using SenacFlix.Domain.Entidades; // Importa a entidade Favorito

namespace SenacFlix.Domain.Interfaces // Define o namespace da camada de dominio, pasta Interfaces
{
    /// <summary>
    /// Contrato que define as operacoes de acesso a dados para a entidade Favorito.
    /// Permite que a camada de Application gerencie listas de favoritos de usuarios
    /// sem depender diretamente de EF Core ou qualquer outro ORM.
    /// </summary>
    public interface IFavoritoRepositorio // interface: apenas assinaturas sem implementacao
    {
        /// <summary>
        /// Retorna todos os filmes favoritados por um usuario especifico.
        /// Inclui os dados do filme para exibicao na lista de favoritos.
        /// </summary>
        /// <param name="usuarioId">Identificador unico do usuario (GUID como string).</param>
        /// <returns>Colecao de registros de favoritos do usuario.</returns>
        Task<IEnumerable<Favorito>> ObterPorUsuarioAsync(string usuarioId); // Busca todos os favoritos de um usuario especifico

        /// <summary>
        /// Retorna um registro de favorito especifico por usuario e filme.
        /// Usado para verificar e recuperar um favorito antes de remover.
        /// Retorna null se o usuario nao favoritou o filme indicado.
        /// </summary>
        /// <param name="usuarioId">Identificador do usuario.</param>
        /// <param name="filmeId">Identificador do filme.</param>
        /// <returns>O favorito encontrado ou null.</returns>
        Task<Favorito?> ObterAsync(string usuarioId, int filmeId); // Favorito? (nullable): retorna null se combinacao nao existir

        /// <summary>
        /// Persiste um novo registro de favorito no banco de dados.
        /// Retorna o favorito com o Id gerado apos a insercao.
        /// </summary>
        /// <param name="favorito">Objeto Favorito a ser inserido.</param>
        /// <returns>O favorito inserido com Id atualizado.</returns>
        Task<Favorito> AdicionarAsync(Favorito favorito); // Adiciona o filme aos favoritos do usuario

        /// <summary>
        /// Remove um registro de favorito do banco de dados.
        /// Diferentemente de filmes e categorias, favoritos sao excluidos fisicamente,
        /// pois nao ha necessidade de historico de desfavoritar.
        /// </summary>
        /// <param name="favorito">Objeto Favorito a ser removido.</param>
        Task RemoverAsync(Favorito favorito); // Hard delete intencional: sem historico de "desfavoritar"

        /// <summary>
        /// Verifica se um usuario ja favoritou um determinado filme.
        /// Metodo de consulta rapida para validacao antes de adicionar duplicatas.
        /// </summary>
        /// <param name="usuarioId">Identificador do usuario.</param>
        /// <param name="filmeId">Identificador do filme.</param>
        /// <returns>true se o filme ja e favorito do usuario; false caso contrario.</returns>
        Task<bool> ExisteAsync(string usuarioId, int filmeId); // Verificacao de existencia para evitar duplicatas na lista
    }
}
