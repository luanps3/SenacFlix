// ============================================================
// Nome:         FormLogin.cs
// Objetivo:     Formulario de entrada da aplicacao responsavel
//               por autenticar o usuario via API e iniciar a sessao.
// Camada:       Apresentacao (Windows Forms)
// Participacao: Ponto de entrada da aplicacao. Exibe campos de
//               login, chama AuthApiCliente e, em caso de sucesso,
//               preenche SessaoUsuario e abre FormPrincipal.
// ============================================================

using System;                              // Necessario para EventArgs e Exception
using System.Drawing;                      // Necessario para Color e Point
using System.Windows.Forms;               // Necessario para Form e controles base
using System.Threading.Tasks;             // Necessario para async/await
using SenacFlix.Desktop.ApiClientes;      // Necessario para AuthApiCliente
using SenacFlix.Desktop.Sessao;           // Necessario para SessaoUsuario
using SenacFlix.Desktop.Forms;            // Necessario para FormPrincipal

namespace SenacFlix.Desktop
{
    /// <summary>
    /// Formulario de login do SenacFlix Desktop.
    /// Responsavel por autenticar o usuario e redirecionar ao painel principal.
    /// </summary>
    public partial class FormLogin : Form
    {
        // --------------------------------------------------------
        // Campos privados
        // --------------------------------------------------------

        // Instancia do cliente HTTP de autenticacao
        private readonly AuthApiCliente _authCliente;

        // Guna2DragControl gerencia o arraste do formulario

        // --------------------------------------------------------
        // Construtor
        // --------------------------------------------------------

        /// <summary>
        /// Inicializa o FormLogin configurando componentes e estilo visual.
        /// </summary>
        public FormLogin()
        {
            // Inicializa todos os componentes definidos no arquivo Designer
            InitializeComponent();

            // Cria a instancia do cliente de autenticacao
            _authCliente = new AuthApiCliente();

            // Aplica estilos adicionais que nao dependem do Designer
            ConfigurarEstilo();

            // Eventos de arraste removidos, Guna2DragControl no Designer faz isso.
        }

        // --------------------------------------------------------
        // Metodo de configuracao de estilo complementar
        // --------------------------------------------------------

        /// <summary>
        /// Configura estilos visuais que complementam o Designer.
        /// Centraliza cores e fontes da paleta Senac.
        /// </summary>
        private void ConfigurarEstilo()
        {
            // Define a cor de fundo principal do formulario (preto-grafico Netflix-like)
            this.BackColor = Color.FromArgb(20, 20, 20);

            // Garante que o formulario nao tenha borda nativa do sistema operacional
            this.FormBorderStyle = FormBorderStyle.None;

            // Centraliza o formulario na tela ao abrir
            this.StartPosition = FormStartPosition.CenterScreen;

            // Define a cor do label de erro como vermelho para destaque visual
            lblErro.ForeColor = Color.FromArgb(220, 53, 69);

            // Garante que o label de erro comece vazio e invisivel
            lblErro.Text = string.Empty;
            lblErro.Visible = false;
        }

        // Eventos de arraste removidos

        // --------------------------------------------------------
        // Evento do botao de login
        // --------------------------------------------------------

        /// <summary>
        /// Executa o processo de autenticacao ao clicar no botao Entrar.
        /// Valida campos, chama a API e redireciona ao painel principal.
        /// </summary>
        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            // Oculta mensagens de erro anteriores antes de nova tentativa
            lblErro.Visible = false;
            lblErro.Text = string.Empty;

            // Valida se o campo de e-mail foi preenchido
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                // Exibe mensagem de erro orientando o usuario
                ExibirErro("Informe o e-mail para acessar o sistema.");
                return; // Interrompe a execucao sem chamar a API
            }

            // Valida se o campo de senha foi preenchido
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                // Exibe mensagem de erro orientando o usuario
                ExibirErro("Informe a senha para acessar o sistema.");
                return; // Interrompe a execucao sem chamar a API
            }

            // Desabilita o botao para evitar cliques multiplos durante o processo
            btnEntrar.Enabled = false;
            btnEntrar.Text = "Aguarde...";

            try
            {
                // Chama a API de autenticacao de forma assincrona
                var resposta = await _authCliente.LoginAsync(
                    txtEmail.Text.Trim(),   // E-mail sem espacos extras
                    txtSenha.Text           // Senha sem alteracao
                );

                // Verifica se a API retornou sucesso
                if (resposta != null && resposta.Sucesso && resposta.Dados != null)
                {
                    // Preenche a sessao global com os dados retornados pela API
                    SessaoUsuario.Instancia.Token       = resposta.Dados.Token;
                    SessaoUsuario.Instancia.NomeUsuario = resposta.Dados.NomeUsuario;
                    SessaoUsuario.Instancia.Email       = resposta.Dados.Email;
                    SessaoUsuario.Instancia.Perfis      = resposta.Dados.Perfis;

                    // Verifica se o usuario tem permissao para acessar o painel administrativo
                    if (!SessaoUsuario.Instancia.EhOperador)
                    {
                        // Limpa a sessao pois o usuario nao tem perfil adequado
                        SessaoUsuario.Instancia.Limpar();

                        // Informa ao usuario sobre a restricao de acesso
                        ExibirErro("Acesso negado. Este painel requer perfil Operador ou Admin.");
                        return; // Nao abre o formulario principal
                    }

                    // Retorna sucesso para o FormPrincipal que chamou este form como Dialog
                    this.DialogResult = DialogResult.OK;

                    // Fecha o formulario de login apos autenticar
                    this.Close();
                }
                else
                {
                    // Exibe a mensagem de erro retornada pela API
                    var mensagemErro = resposta?.Mensagem ?? "Credenciais invalidas. Tente novamente.";
                    ExibirErro(mensagemErro);
                }
            }
            catch (Exception ex)
            {
                // Exibe erro de comunicacao com a API (ex: servidor offline)
                ExibirErro($"Erro ao conectar com o servidor: {ex.Message}");
            }
            finally
            {
                // Reabilita o botao e restaura o texto original independente do resultado
                btnEntrar.Enabled = true;
                btnEntrar.Text = "Entrar";
            }
        }

        // --------------------------------------------------------
        // Evento do botao fechar
        // --------------------------------------------------------

        /// <summary>
        /// Encerra completamente a aplicacao ao clicar no botao de fechar.
        /// </summary>
        private void btnFechar_Click(object sender, EventArgs e)
        {
            // Encerra o processo da aplicacao Windows Forms
            Application.Exit();
        }

        // --------------------------------------------------------
        // Metodo auxiliar de exibicao de erro
        // --------------------------------------------------------

        /// <summary>
        /// Exibe uma mensagem de erro no label reservado para esse fim.
        /// </summary>
        /// <param name="mensagem">Texto do erro a ser exibido ao usuario.</param>
        private void ExibirErro(string mensagem)
        {
            // Define o texto da mensagem de erro
            lblErro.Text = mensagem;

            // Torna o label visivel para o usuario
            lblErro.Visible = true;
        }
    }
}
