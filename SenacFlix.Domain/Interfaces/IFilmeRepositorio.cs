// ============================================================
// Nome:         IFilmeRepositorio.cs
// Objetivo:     Define o contrato (interface) que qualquer implementacao
//               de repositorio de filmes deve seguir no SenacFlix.
//               Abstrai o acesso a dados e permite inversao de dependencia.
// Camada:       Domain (Interfaces)
// Participa em: Implementada pela camada Infrastructure (EF Core).
//               Injetada e consumida pela camada Application (servicos/casos de uso).
//               Respeita o principio D do SOLID (Dependency Inversion Principle).
// ============================================================

using SenacFlix.Domain.Entidades; // Importa a entidade Filme para usar nos metodos

namespace SenacFlix.Domain.Interfaces // Define o namespace da camada de dominio, pasta Interfaces
{
    /// <summary>
    /// Contrato que define todas as operacoes de acesso a dados da entidade Filme.
    /// A implementacao concreta (usando EF Core, Dapper, etc.) fica na Infrastructure.
    /// A camada de Application depende desta interface, nunca da implementacao concreta.
    /// </summary>
    public interface IFilmeRepositorio // interface: define apenas assinaturas de metodos, sem implementacao
    {
        /// <summary>
        /// Retorna todos os filmes do catalogo de forma assincrona.
        /// Por padrao, retorna apenas filmes ativos (Ativo = true).
        /// Quando incluirInativos = true, retorna todos, incluindo desativados.
        /// </summary>
        /// <param name="incluirInativos">Se true, inclui filmes inativos no resultado.</param>
        /// <returns>Colecao enumeravel de todos os filmes encontrados.</returns>
        Task<IEnumerable<Filme>> ObterTodosAsync(bool incluirInativos = false); // Task<T>: metodo assincrono que retorna dados ao completar

        /// <summary>
        /// Retorna um unico filme pelo seu identificador.
        /// Retorna null se o filme nao for encontrado ou estiver inativo.
        /// </summary>
        /// <param name="id">Identificador unico do filme.</param>
        /// <returns>O filme encontrado ou null.</returns>
        Task<Filme?> ObterPorIdAsync(int id); // Filme? (nullable): retorna null se nao encontrado

        /// <summary>
        /// Busca filmes cujo titulo, diretor ou elenco contenham o termo informado.
        /// Pesquisa case-insensitive (ignora maiusculas/minusculas).
        /// </summary>
        /// <param name="termo">Texto a ser buscado nos campos do filme.</param>
        /// <returns>Colecao de filmes que correspondem ao termo de busca.</returns>
        Task<IEnumerable<Filme>> BuscarAsync(string? termo, int? categoriaId = null);

        /// <summary>
        /// Retorna todos os filmes ativos pertencentes a uma categoria especifica.
        /// </summary>
        /// <param name="categoriaId">Identificador da categoria a filtrar.</param>
        /// <returns>Colecao de filmes da categoria informada.</returns>
        Task<IEnumerable<Filme>> ObterPorCategoriaAsync(int categoriaId); // Filtro por FK de categoria

        /// <summary>
        /// Persiste um novo filme no banco de dados.
        /// Retorna o filme com o Id gerado pelo banco apos a insercao.
        /// </summary>
        /// <param name="filme">Objeto Filme preenchido com os dados a serem salvos.</param>
        /// <returns>O filme inserido com Id atualizado.</returns>
        Task<Filme> AdicionarAsync(Filme filme); // Insere e retorna o objeto com Id gerado

        /// <summary>
        /// Atualiza os dados de um filme existente no banco de dados.
        /// </summary>
        /// <param name="filme">Objeto Filme com os dados atualizados.</param>
        Task AtualizarAsync(Filme filme); // Task sem retorno: operacao de escrita pura

        /// <summary>
        /// Realiza a exclusao logica do filme: seta Ativo = false e preenche DataExclusao.
        /// O registro permanece no banco de dados, mas nao e mais exibido aos usuarios.
        /// </summary>
        /// <param name="id">Identificador do filme a ser desativado.</param>
        Task DesativarAsync(int id); // Soft delete: mantem o registro no banco mas o oculta

        /// <summary>
        /// Remove permanentemente o registro do filme do banco de dados.
        /// Operacao irreversivel; usar com cautela e apenas quando estritamente necessario.
        /// </summary>
        /// <param name="id">Identificador do filme a ser excluido permanentemente.</param>
        Task ExcluirPermanentementeAsync(int id); // Hard delete: remove fisicamente o registro; irreversivel

        /// <summary>
        /// Reativa um filme previamente desativado (Soft Delete).
        /// Torna o registro visível e utilizável novamente.
        /// </summary>
        /// <param name="id">Identificador do filme a ser reativado.</param>
        Task ReativarAsync(int id);
    }
}
