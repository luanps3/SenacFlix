// ============================================================
// Nome:         FilmeApiCliente.cs
// Objetivo:     Realizar todas as chamadas HTTP relacionadas ao
//               gerenciamento de filmes na API do SenacFlix.
// Camada:       ApiClientes (infraestrutura de comunicacao)
// Participacao: Utilizado pelos UserControls de filmes para
//               listar, buscar, cadastrar, atualizar e remover
//               filmes via API REST autenticada.
// ============================================================

using System.Collections.Generic;  // Necessario para List<FilmeDto>
using System.Threading.Tasks;      // Necessario para operacoes assincronas

namespace SenacFlix.Desktop.ApiClientes
{
    // ============================================================
    // DTO do Filme utilizado em toda a camada Desktop
    // ============================================================

    /// <summary>
    /// Objeto de transferencia de dados do Filme.
    /// Representa os campos enviados e recebidos pela API REST.
    /// </summary>
    public class FilmeDto
    {
        // Identificador unico do filme no banco de dados
        public int Id { get; set; }

        // Titulo completo do filme (ex: "Matrix")
        public string Titulo { get; set; }

        // Sinopse ou descricao detalhada do filme
        public string Descricao { get; set; }

        // Ano de lancamento do filme (ex: 1999)
        public int AnoLancamento { get; set; }

        // Duracao do filme em minutos
        public int Duracao { get; set; }

        // Nome do diretor do filme
        public string Diretor { get; set; }

        // Elenco principal separado por virgula
        public string Elenco { get; set; }

        // Identificador da categoria/genero do filme
        public int CategoriaId { get; set; }

        // Nome da categoria do filme (preenchido na resposta da API)
        public string CategoriaNome { get; set; }

        // Identificador da classificacao indicativa (ex: 12, 16, 18)
        public int ClassificacaoIndicativaId { get; set; }

        // Nome da classificacao indicativa (preenchido na resposta da API)
        public string ClassificacaoNome { get; set; }

        // Indica se o filme esta ativo (visivel) ou desativado
        public bool Ativo { get; set; }

        // URL da capa/imagem do filme (opcional)
        public string ImagemCapaUrl { get; set; }

        public string ImagemBannerUrl { get; set; }
        public string TrailerYoutubeUrl { get; set; }
        public string VideoYoutubeUrl { get; set; }
        public bool DestaqueHome { get; set; }
        public System.DateTime DataCadastro { get; set; }
    }

    // ============================================================
    // Cliente HTTP especializado em operacoes de filmes
    // ============================================================

    /// <summary>
    /// Classe responsavel por toda comunicacao HTTP relativa a filmes.
    /// Herda de ClienteHttp para reutilizar autenticacao e serializacao.
    /// </summary>
    public class FilmeApiCliente : ClienteHttp
    {
        // --------------------------------------------------------
        // Prefixo base das rotas de filmes
        // --------------------------------------------------------

        // Prefixo comum a todos os endpoints de filmes
        private const string RotaBase = "/api/filmes";

        // --------------------------------------------------------
        // Metodos de consulta (leitura)
        // --------------------------------------------------------

        /// <summary>
        /// Obtem a lista completa de filmes cadastrados na plataforma.
        /// Requer autenticacao JWT.
        /// </summary>
        /// <returns>Lista de FilmeDto com todos os filmes.</returns>
        public async Task<List<FilmeDto>> ObterTodosAsync()
        {
            var resposta = await GetAsync<ApiRespostaSimples<List<FilmeDto>>>($"{RotaBase}/todos");
            return resposta?.Dados ?? new List<FilmeDto>();
        }

        /// <summary>
        /// Obtem os dados de um filme especifico pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador unico do filme.</param>
        /// <returns>FilmeDto com os dados do filme encontrado.</returns>
        public async Task<FilmeDto> ObterPorIdAsync(int id)
        {
            var resposta = await GetAsync<ApiRespostaSimples<FilmeDto>>($"{RotaBase}/{id}");
            return resposta?.Dados;
        }

        /// <summary>
        /// Busca filmes pelo titulo ou outros criterios de texto.
        /// </summary>
        /// <param name="termo">Texto a ser pesquisado nos filmes.</param>
        /// <returns>Lista de FilmeDto correspondente ao termo de busca.</returns>
        public async Task<List<FilmeDto>> BuscarAsync(string termo, int? categoriaId = null)
        {
            var url = $"{RotaBase}/buscar?termo={termo}";
            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                url += $"&categoriaId={categoriaId.Value}";
            }
            var resposta = await GetAsync<ApiRespostaSimples<List<FilmeDto>>>(url);
            return resposta?.Dados ?? new List<FilmeDto>();
        }

        // --------------------------------------------------------
        // Metodos de escrita (criacao e atualizacao)
        // --------------------------------------------------------

        /// <summary>
        /// Envia os dados de um novo filme para cadastro na API.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="dados">FilmeDto com os dados do novo filme.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<FilmeDto>> CadastrarAsync(FilmeDto dados)
        {
            // Chama POST /api/filmes enviando o DTO do novo filme no corpo
            return await PostAsync<ApiRespostaSimples<FilmeDto>>(RotaBase, dados);
        }

        /// <summary>
        /// Atualiza os dados de um filme existente na API.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="id">Identificador do filme a ser atualizado.</param>
        /// <param name="dados">FilmeDto com os novos dados do filme.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<FilmeDto>> AtualizarAsync(int id, FilmeDto dados)
        {
            // Chama PUT /api/filmes/{id} com os dados atualizados no corpo
            return await PutAsync<ApiRespostaSimples<FilmeDto>>($"{RotaBase}/{id}", dados);
        }

        // --------------------------------------------------------
        // Metodos de remocao (desativacao e exclusao)
        // --------------------------------------------------------

        /// <summary>
        /// Desativa logicamente um filme sem excluir do banco de dados.
        /// O filme fica invisivel para o usuario mas pode ser reativado.
        /// Requer perfil Admin ou Operador.
        /// </summary>
        /// <param name="id">Identificador do filme a ser desativado.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<object>> DesativarAsync(int id)
        {
            // Chama DELETE /api/filmes/{id}/desativar para desativacao logica
            return await DeleteAsync<ApiRespostaSimples<object>>($"{RotaBase}/{id}/desativar");
        }

        /// <summary>
        /// Remove permanentemente um filme do banco de dados.
        /// Operacao irreversivel. Requer perfil Admin exclusivamente.
        /// </summary>
        /// <param name="id">Identificador do filme a ser excluido definitivamente.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<object>> ExcluirPermanentementeAsync(int id)
        {
            // Chama DELETE /api/filmes/{id}/permanente para exclusao fisca do banco
            return await DeleteAsync<ApiRespostaSimples<object>>($"{RotaBase}/{id}/permanente");
        }
    }
}
