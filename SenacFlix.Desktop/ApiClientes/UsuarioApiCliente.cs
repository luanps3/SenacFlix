// ============================================================
// Nome:         UsuarioApiCliente.cs
// Objetivo:     Realizar chamadas HTTP relacionadas ao
//               gerenciamento de usuarios na API do SenacFlix.
// Camada:       ApiClientes (infraestrutura de comunicacao)
// Participacao: Utilizado pelo UserControlUsuarios para
//               listar e desativar usuarios. Acesso restrito
//               ao perfil Admin.
// ============================================================

using System.Collections.Generic;  // Necessario para List<UsuarioDto>
using System.Threading.Tasks;      // Necessario para operacoes assincronas

namespace SenacFlix.Desktop.ApiClientes
{
    // ============================================================
    // DTO do Usuario utilizado em toda a camada Desktop
    // ============================================================

    /// <summary>
    /// Objeto de transferencia de dados do Usuario.
    /// Representa os campos recebidos da API na listagem de usuarios.
    /// </summary>
    public class UsuarioDto
    {
        // Identificador unico do usuario (GUID em formato string)
        public string Id { get; set; }

        // Nome completo do usuario (ex: "Carlos Andrade")
        public string NomeCompleto { get; set; }

        // Endereco de e-mail do usuario
        public string Email { get; set; }

        // Lista de perfis do usuario (ex: ["Admin"], ["Operador"])
        public List<string> Perfis { get; set; }

        // Indica se o usuario esta ativo na plataforma
        public bool Ativo { get; set; }
        
        public System.DateTime DataCadastro { get; set; }

        public string? FotoPerfilUrl { get; set; }
    }

    // ============================================================
    // Cliente HTTP especializado em operacoes de usuarios
    // ============================================================

    /// <summary>
    /// Classe responsavel por toda comunicacao HTTP relativa a usuarios.
    /// Herda de ClienteHttp para reutilizar autenticacao e serializacao.
    /// </summary>
    public class UsuarioApiCliente : ClienteHttp
    {
        // --------------------------------------------------------
        // Prefixo base das rotas de usuarios
        // --------------------------------------------------------

        // Prefixo comum a todos os endpoints de usuarios
        private const string RotaBase = "/api/usuarios";

        // --------------------------------------------------------
        // Metodos de consulta (leitura)
        // --------------------------------------------------------

        /// <summary>
        /// Obtem a lista de todos os usuarios cadastrados na plataforma.
        /// Disponivel apenas para perfil Admin.
        /// </summary>
        /// <returns>Lista de UsuarioDto com todos os usuarios.</returns>
        public async Task<List<UsuarioDto>> ObterTodosAsync()
        {
            var resposta = await GetAsync<ApiRespostaSimples<List<UsuarioDto>>>(RotaBase);
            return resposta?.Dados ?? new List<UsuarioDto>();
        }

        // --------------------------------------------------------
        // Metodo de desativacao (remocao logica)
        // --------------------------------------------------------

        /// <summary>
        /// Desativa logicamente um usuario sem excluir do banco de dados.
        /// O usuario perde acesso a plataforma mas os dados sao mantidos.
        /// Disponivel apenas para perfil Admin.
        /// </summary>
        /// <param name="id">Identificador GUID do usuario a ser desativado.</param>
        /// <returns>ApiRespostaSimples indicando sucesso ou falha.</returns>
        public async Task<ApiRespostaSimples<object>> DesativarAsync(string id)
        {
            // Chama DELETE /api/usuarios/{id} para desativar o usuario na API
            return await DeleteAsync<ApiRespostaSimples<object>>($"{RotaBase}/{id}");
        }
    }
}
