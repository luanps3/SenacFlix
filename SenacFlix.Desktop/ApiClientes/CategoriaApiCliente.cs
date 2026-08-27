// ============================================================
// Nome:         CategoriaApiCliente.cs
// Objetivo:     Realizar todas as chamadas HTTP relacionadas ao
//               gerenciamento de categorias na API do SenacFlix.
// Camada:       ApiClientes (infraestrutura de comunicacao)
// Participacao: Utilizado pelos UserControls de categorias e
//               pelo formulario de cadastro de filmes para
//               preencher o combo de generos.
// ============================================================

using System.Collections.Generic;  // Necessario para List<CategoriaDto>
using System.Threading.Tasks;      // Necessario para operacoes assincronas

namespace SenacFlix.Desktop.ApiClientes
{
    // ============================================================
    // DTO da Categoria utilizado em toda a camada Desktop
    // ============================================================

    /// <summary>
    /// Objeto de transferencia de dados da Categoria.
    /// Representa os campos enviados e recebidos pela API REST.
    /// </summary>
    public class CategoriaDto
    {
        // Identificador unico da categoria no banco de dados
        public int Id { get; set; }

        // Nome da categoria (ex: "Acao", "Comedia", "Drama")
        public string Nome { get; set; }

        // Descricao detalhada da categoria
        public string Descricao { get; set; }

        // Indica se a categoria esta ativa (disponivel para associacao a filmes)
        public bool Ativo { get; set; }
    }

    // ============================================================
    // Cliente HTTP especializado em operacoes de categorias
    // ============================================================

    /// <summary>
    /// Classe responsavel por toda comunicacao HTTP relativa a categorias.
    /// Herda de ClienteHttp para reutilizar autenticacao e serializacao.
    /// </summary>
    public class CategoriaApiCliente : ClienteHttp
    {
        // --------------------------------------------------------
        // Prefixo base das rotas de categorias
        // --------------------------------------------------------

        // Prefixo comum a todos os endpoints de categorias
        private const string RotaBase = "/api/categorias";

        // --------------------------------------------------------
        // Metodos de consulta (leitura)
        // --------------------------------------------------------

        /// <summary>
        /// Obtem a lista de todas as categorias cadastradas na plataforma.
        /// Utilizado tanto na listagem quanto no combo do formulario de filmes.
        /// </summary>
        /// <returns>Lista de CategoriaDto com todas as categorias.</returns>
        public async Task<List<CategoriaDto>> ObterTodasAsync()
        {
            var resposta = await GetAsync<ApiRespostaSimples<List<CategoriaDto>>>(RotaBase);
            return resposta?.Dados ?? new List<CategoriaDto>();
        }

        // --------------------------------------------------------
        // Metodos de escrita (criacao e atualizacao)
        // --------------------------------------------------------

        /// <summary>
        /// Obtem os dados de uma categoria pelo seu ID.
        /// </summary>
        public async Task<CategoriaDto> ObterPorIdAsync(int id)
        {
            var resposta = await GetAsync<ApiRespostaSimples<CategoriaDto>>($"{RotaBase}/{id}");
            return resposta?.Dados;
        }

        /// <summary>
        /// Envia os dados de uma nova categoria para cadastro na API.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="dados">CategoriaDto com os dados da nova categoria.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha no cadastro.</returns>
        public async Task<ApiRespostaSimples<CategoriaDto>> CadastrarAsync(CategoriaDto dados)
        {
            // Chama POST /api/categorias enviando o DTO da nova categoria no corpo
            return await PostAsync<ApiRespostaSimples<CategoriaDto>>(RotaBase, dados);
        }

        /// <summary>
        /// Atualiza os dados de uma categoria existente na API.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser atualizada.</param>
        /// <param name="dados">CategoriaDto com os novos dados.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha na atualizacao.</returns>
        public async Task<ApiRespostaSimples<CategoriaDto>> AtualizarAsync(int id, CategoriaDto dados)
        {
            // Chama PUT /api/categorias/{id} com os dados atualizados no corpo
            return await PutAsync<ApiRespostaSimples<CategoriaDto>>($"{RotaBase}/{id}", dados);
        }

        // --------------------------------------------------------
        // Metodo de desativacao (remocao logica)
        // --------------------------------------------------------

        /// <summary>
        /// Desativa logicamente uma categoria sem excluir do banco de dados.
        /// Categorias desativadas nao aparecem no combo de cadastro de filmes.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="id">Identificador da categoria a ser desativada.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<object>> DesativarAsync(int id)
        {
            // Chama DELETE /api/categorias/{id}/desativar para desativacao logica
            return await DeleteAsync<ApiRespostaSimples<object>>($"{RotaBase}/{id}/desativar");
        }
    }
}
