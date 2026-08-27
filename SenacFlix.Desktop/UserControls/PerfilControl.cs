using System;
using System.Windows.Forms;
using SenacFlix.Desktop.Sessao;

namespace SenacFlix.Desktop.UserControls
{
    public partial class PerfilControl : UserControl
    {
        public PerfilControl()
        {
            InitializeComponent();
        }

        private void PerfilControl_Load(object sender, EventArgs e)
        {
            CarregarDadosPerfil();
        }

        private void CarregarDadosPerfil()
        {
            if (SessaoUsuario.Instancia.Token != null)
            {
                lblNome.Text = SessaoUsuario.Instancia.NomeUsuario;
                lblEmail.Text = SessaoUsuario.Instancia.Email;
                
                if (SessaoUsuario.Instancia.Perfis != null && SessaoUsuario.Instancia.Perfis.Count > 0)
                {
                    lblPerfis.Text = string.Join(", ", SessaoUsuario.Instancia.Perfis);
                }
                else
                {
                    lblPerfis.Text = "Sem perfis associados";
                }
            }
        }
    }
}
