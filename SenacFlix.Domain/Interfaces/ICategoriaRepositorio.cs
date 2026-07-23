// ============================================================
// Nome:         ICategoriaRepositorio.cs
// Objetivo:     Define o contrato de acesso a dados para a entidade Categoria
//               no SenacFlix. Permite que a camada de Application opere sobre
//               categorias sem conhecer os detalhes de implementacao do banco.
// Camada:       Domain (Interfaces)
// Participa em: Implementada pela camada Infrastructure.
//               Consumida pela camada Application nos servicos de categoria.
// ============================================================

using SenacFlix.Domain.Entidades; // Importa a entidade Categoria

namespace SenacFlix.Domain.Interfaces // Define o namespace da camada de dominio, pasta Interfaces
{
    /// <summary>
    /// Contrato que define as operacoes de acesso a dados para a entidade Categoria.
    /// A camada de Application depende desta interface para manipular categorias,
    /// sem acoplamento com a tecnologia de banco de dados utilizada.
    /// </summary>
    public interface ICategoriaRepositorio // interface: contrato sem implementacao
    {
        /// <summary>
        /// Retorna todas as categorias de forma assincrona.
        /// Por padrao, retorna apenas categorias ativas.
        /// Quando incluirInativas = true, retorna todas, incluindo as desativadas.
        /// </summary>
        /// <param name="incluirInativas">Se true, inclui categorias inativas no resultado.</param>
        /// <returns>Colecao enumeravel de categorias.</returns>
        Task<IEnumerable<Categoria>> ObterTodasAsync(bool incluirInativas = false); // Parametro opcional com valor padrao false

        /// <summary>
        /// Retorna uma categoria especifica pelo seu identificador.
        /// Retorna null se nao encontrada.
        /// </summary>
        /// <param name="id">Identificador unico da categoria.</param>
        /// <returns>A categoria encontrada ou null.</returns>
        Task<Categoria?> ObterPorIdAsync(int id); // Categoria? (nullable): retorna null se a categoria nao existir

        /// <summary>
        /// Persiste uma nova categoria no banco de dados.
        /// Retorna a categoria com o Id gerado pelo banco apos a insercao.
        /// </summary>
        /// <param name="categoria">Objeto Categoria a ser inserido.</param>
        /// <returns>A categoria inserida com Id atualizado.</returns>
        Task<Categoria> AdicionarAsync(Categoria categoria); // Insercao e retorno do objeto com Id gerado

        /// <summary>
        /// Atualiza os dados de uma categoria existente no banco de dados.
        /// </summary>
        /// <param name="categoria">Objeto Categoria com os dados atualizados.</param>
        Task AtualizarAsync(Categoria categoria); // Task sem retorno: operacao de escrita

        /// <summary>
        /// Realiza a exclusao logica da categoria: seta Ativo = false.
        /// Categorias desativadas nao aparecem nas listas da plataforma.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser desativada.</param>
        Task DesativarAsync(int id); // Soft delete: mantem o registro no banco

        /// <summary>
        /// Reativa uma categoria desativada: seta Ativo = true.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser reativada.</param>
        Task ReativarAsync(int id); // Reativa o registro no banco

        /// <summary>
        /// Remove permanentemente o registro da categoria do banco de dados.
        /// Atencao: so deve ser executado se nao houver filmes vinculados a esta categoria.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser excluida permanentemente.</param>
        Task ExcluirPermanentementeAsync(int id); // Hard delete: excluir apenas se nao houver filmes vinculados
    }
}