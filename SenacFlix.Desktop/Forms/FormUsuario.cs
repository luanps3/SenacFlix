using System;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;

namespace SenacFlix.Desktop.Forms
{
    public partial class FormUsuario : Form
    {
        private UsuarioApiCliente _usuarioCliente;
        private string _usuarioId;

        public FormUsuario(string usuarioId = null)
        {
            InitializeComponent();
            _usuarioCliente = new UsuarioApiCliente();
            _usuarioId = usuarioId;
            
            if (!string.IsNullOrEmpty(_usuarioId))
            {
                lblTitle.Text = "Editar Usuário";
                // Idealmente buscaria o usuario na API
                MessageBox.Show("Endpoint de edição ainda não disponível na API.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblTitle.Text = "Novo Usuário";
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Nome e E-mail são obrigatórios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Simulação de Cadastro/Atualização: Endpoint para gerenciar usuário pela área administrativa ainda não foi implementado na API. Esta tela é apenas representação visual para conclusão da etapa Desktop.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
