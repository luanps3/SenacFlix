using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;
using SenacFlix.Desktop.Sessao;

namespace SenacFlix.Desktop.UserControls
{
    public partial class UsuariosControl : UserControl
    {
        private UsuarioApiCliente _usuarioCliente;

        public UsuariosControl()
        {
            InitializeComponent();
            _usuarioCliente = new UsuarioApiCliente();
        }

        private async void UsuariosControl_Load(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
            ConfigurarPermissoes();
        }

        private void ConfigurarPermissoes()
        {
            // Apenas Admin pode ver usuários ou gerenciar, o menu já barra,
            // mas podemos colocar restrições extras aqui.
            // Ex: Apenas Admin pode adicionar/excluir
            btnAdicionar.Visible = SessaoUsuario.Instancia.EhAdmin;
            btnExcluir.Visible = SessaoUsuario.Instancia.EhAdmin;
        }

        private async Task CarregarDadosAsync(string pesquisa = "")
        {
            try
            {
                var usuarios = await _usuarioCliente.ObterTodosAsync();
                
                if (!string.IsNullOrWhiteSpace(pesquisa) && usuarios != null)
                {
                    usuarios = usuarios.FindAll(u => u.NomeCompleto.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                     u.Email.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                dgvUsuarios.DataSource = usuarios;
                
                if (dgvUsuarios.Columns.Contains("Id"))
                    dgvUsuarios.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar usuários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync(txtPesquisa.Text);
        }

        private async void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            await CarregarDadosAsync(txtPesquisa.Text);
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            var form = new SenacFlix.Desktop.Forms.FormUsuario();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CarregarDadosAsync(txtPesquisa.Text);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var id = dgvUsuarios.SelectedRows[0].Cells["Id"].Value.ToString();
                var form = new SenacFlix.Desktop.Forms.FormUsuario(id);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _ = CarregarDadosAsync(txtPesquisa.Text);
                }
            }
            else
            {
                MessageBox.Show("Selecione um usuário para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                var id = (int)dgvUsuarios.SelectedRows[0].Cells["Id"].Value;
                var nome = dgvUsuarios.SelectedRows[0].Cells["NomeCompleto"].Value.ToString();
                
                var result = MessageBox.Show($"Deseja desativar/excluir o usuário '{nome}'?", "Confirmar Ação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var response = await _usuarioCliente.DesativarAsync(id.ToString());
                        if (response.Sucesso)
                        {
                            MessageBox.Show("Usuário excluído/desativado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await CarregarDadosAsync(txtPesquisa.Text);
                        }
                        else
                        {
                            MessageBox.Show($"Erro: {response.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro na requisição: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
