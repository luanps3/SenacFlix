// ============================================================
// Nome:         SessaoUsuario.cs
// Objetivo:     Manter os dados do usuario autenticado em memoria
//               durante toda a execucao da aplicacao Desktop.
// Camada:       Sessao (estado global da aplicacao)
// Participacao: Singleton acessado por qualquer formulario ou
//               UserControl para verificar identidade e permissoes.
// ============================================================

using System.Collections.Generic; // Necessario para List<string>

namespace SenacFlix.Desktop.Sessao
{
    /// <summary>
    /// Classe singleton que armazena os dados da sessao do usuario logado.
    /// Apenas uma instancia desta classe existe durante toda a execucao.
    /// </summary>
    public class SessaoUsuario
    {
        // --------------------------------------------------------
        // Instancia unica da classe (padrao Singleton thread-safe)
        // --------------------------------------------------------

        // Campo privado estatico que guarda a unica instancia da classe
        private static SessaoUsuario _instancia;

        // Objeto de travamento para garantir seguranca em ambiente multithread
        private static readonly object _trava = new object();

        // --------------------------------------------------------
        // Construtor privado: impede que outros criem instancias
        // --------------------------------------------------------

        /// <summary>
        /// Construtor privado - impede instanciacao externa (padrao Singleton).
        /// </summary>
        private SessaoUsuario()
        {
            // Inicializa a lista de perfis como lista vazia para evitar NullReference
            Perfis = new List<string>();
        }

        // --------------------------------------------------------
        // Propriedade publica para acessar a instancia unica
        // --------------------------------------------------------

        /// <summary>
        /// Retorna a instancia unica da SessaoUsuario.
        /// Cria a instancia na primeira chamada (lazy initialization).
        /// </summary>
        public static SessaoUsuario Instancia
        {
            get
            {
                // Verifica se a instancia ja foi criada (verificacao dupla para performance)
                if (_instancia == null)
                {
                    // Entra em regiao critica para evitar criacao duplicada em multithread
                    lock (_trava)
                    {
                        // Verifica novamente dentro do lock para garantir unicidade
                        if (_instancia == null)
                        {
                            // Cria a unica instancia da sessao
                            _instancia = new SessaoUsuario();
                        }
                    }
                }

                // Retorna a instancia existente ou recem-criada
                return _instancia;
            }
        }

        // --------------------------------------------------------
        // Propriedades de dados da sessao do usuario
        // --------------------------------------------------------

        /// <summary>
        /// Token JWT retornado pela API apos o login bem-sucedido.
        /// Enviado no cabecalho Authorization de cada requisicao.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Nome de exibicao do usuario logado (ex: "Carlos Andrade").
        /// Exibido na sidebar do FormPrincipal.
        /// </summary>
        public string NomeUsuario { get; set; }

        /// <summary>
        /// Endereco de e-mail do usuario logado.
        /// Exibido no UserControlPerfil.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Lista de perfis/roles do usuario (ex: "Admin", "Operador").
        /// Controla o que o usuario pode ver e fazer na aplicacao.
        /// </summary>
        public List<string> Perfis { get; set; }

        // --------------------------------------------------------
        // Propriedades calculadas de permissao
        // --------------------------------------------------------

        /// <summary>
        /// Retorna verdadeiro se o usuario possui o perfil "Admin".
        /// Admins tem acesso total, incluindo exclusao permanente.
        /// </summary>
        public bool EhAdmin => Perfis != null && Perfis.Contains("Admin");

        /// <summary>
        /// Retorna verdadeiro se o usuario e Operador OU Admin.
        /// Operadores podem cadastrar e desativar, mas nao excluir permanentemente.
        /// </summary>
        public bool EhOperador => (Perfis != null && Perfis.Contains("Operador")) || EhAdmin;

        // --------------------------------------------------------
        // Metodo para encerrar a sessao (logout)
        // --------------------------------------------------------

        /// <summary>
        /// Limpa todos os dados da sessao ao fazer logout.
        /// Chamado antes de reiniciar a aplicacao.
        /// </summary>
        public void Limpar()
        {
            // Remove o token JWT da memoria
            Token = null;

            // Limpa o nome do usuario
            NomeUsuario = null;

            // Limpa o e-mail
            Email = null;

            // Redefine a lista de perfis como vazia
            Perfis = new List<string>();
        }
    }
}
