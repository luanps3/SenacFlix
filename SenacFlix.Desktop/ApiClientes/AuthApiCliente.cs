// ============================================================
// Nome:         AuthApiCliente.cs
// Objetivo:     Realizar chamadas HTTP relacionadas a autenticacao
//               de usuarios na API do SenacFlix.
// Camada:       ApiClientes (infraestrutura de comunicacao)
// Participacao: Utilizado pelo FormLogin para autenticar o usuario
//               e obter o token JWT da sessao.
// ============================================================

using System.Collections.Generic;  // Necessario para List<string> nos perfis
using System.Threading.Tasks;      // Necessario para operacoes assincronas

namespace SenacFlix.Desktop.ApiClientes
{
    // ============================================================
    // DTOs (Data Transfer Objects) utilizados neste cliente
    // ============================================================

    /// <summary>
    /// Objeto enviado ao endpoint de login com as credenciais do usuario.
    /// </summary>
    public class LoginDto
    {
        // E-mail do usuario para autenticacao
        public string Email { get; set; }

        // Senha do usuario para autenticacao
        public string Senha { get; set; }
    }

    /// <summary>
    /// Dados retornados pela API apos login bem-sucedido.
    /// Contém o token JWT e informacoes basicas do usuario.
    /// </summary>
    public class LoginRespostaDto
    {
        // Token JWT gerado pelo servidor para autorizar requisicoes futuras
        public string Token { get; set; }

        // Data e hora de expiracao do token JWT
        public string Expiracao { get; set; }

        // Nome de exibicao do usuario autenticado
        public string NomeUsuario { get; set; }

        // Endereco de e-mail do usuario autenticado
        public string Email { get; set; }

        // Lista de perfis/roles do usuario (ex: "Admin", "Operador")
        public List<string> Perfis { get; set; }
    }

    /// <summary>
    /// Envelope padrao de resposta da API para operacoes com retorno de dados.
    /// Permite verificar sucesso/erro e acessar a mensagem e os dados.
    /// </summary>
    /// <typeparam name="T">Tipo dos dados retornados pela API.</typeparam>
    public class ApiRespostaSimples<T>
    {
        // Indica se a operacao foi concluida com sucesso
        public bool Sucesso { get; set; }

        // Mensagem descritiva do resultado (ex: "Login realizado com sucesso")
        public string Mensagem { get; set; }

        // Dados retornados pela operacao (pode ser null em caso de erro)
        public T Dados { get; set; }
    }

    // ============================================================
    // Classe principal do cliente de autenticacao
    // ============================================================

    /// <summary>
    /// Cliente HTTP especializado em chamadas de autenticacao.
    /// Herda de ClienteHttp para reutilizar a infraestrutura HTTP.
    /// </summary>
    public class AuthApiCliente : ClienteHttp
    {
        // --------------------------------------------------------
        // Rotas da API de autenticacao
        // --------------------------------------------------------

        // Rota do endpoint de login da API
        private const string RotaLogin = "/api/autenticacao/login";

        // --------------------------------------------------------
        // Metodos publicos do cliente
        // --------------------------------------------------------

        /// <summary>
        /// Envia as credenciais do usuario para a API e retorna o resultado do login.
        /// Nao requer token JWT pois e uma rota publica.
        /// </summary>
        /// <param name="email">E-mail digitado pelo usuario no formulario de login.</param>
        /// <param name="senha">Senha digitada pelo usuario no formulario de login.</param>
        /// <returns>ApiRespostaSimples com LoginRespostaDto em caso de sucesso.</returns>
        public async Task<ApiRespostaSimples<LoginRespostaDto>> LoginAsync(string email, string senha)
        {
            // Monta o objeto DTO com as credenciais do usuario
            var credenciais = new LoginDto
            {
                Email = email,  // Define o e-mail para autenticacao
                Senha = senha   // Define a senha para autenticacao
            };

            // Chama o metodo POST sem autenticacao (rota publica de login)
            // Retorna o envelope ApiRespostaSimples contendo o token e dados do usuario
            return await PostSemAutenticacaoAsync<ApiRespostaSimples<LoginRespostaDto>>(
                RotaLogin,      // Rota do endpoint de login
                credenciais     // Corpo da requisicao com as credenciais
            );
        }
    }
}
