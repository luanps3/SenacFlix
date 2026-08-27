using System;
using System.Windows.Forms;
using SenacFlix.Desktop.Sessao;

namespace SenacFlix.Desktop.Forms
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            CarregarDadosUsuario();
            CarregarUserControl(new UserControls.DashboardControl());
        }

        private void CarregarDadosUsuario()
        {
            if (SessaoUsuario.Instancia.Token != null)
            {
                lblUserName.Text = SessaoUsuario.Instancia.NomeUsuario;
                lblUserRole.Text = SessaoUsuario.Instancia.Perfis != null && SessaoUsuario.Instancia.Perfis.Count > 0 ? string.Join(", ", SessaoUsuario.Instancia.Perfis) : "Usuário";
            }
        }

        private void CarregarUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(uc);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            CarregarUserControl(new UserControls.DashboardControl());
        }

        private void btnFilmes_Click(object sender, EventArgs e)
        {
            CarregarUserControl(new UserControls.FilmesControl());
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            CarregarUserControl(new UserControls.CategoriasControl());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            CarregarUserControl(new UserControls.UsuariosControl());
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            CarregarUserControl(new UserControls.PerfilControl());
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            SessaoUsuario.Instancia.Limpar();
            this.Hide();
            var formLogin = new FormLogin();
            formLogin.ShowDialog();
            this.Close();
        }
    }
}
