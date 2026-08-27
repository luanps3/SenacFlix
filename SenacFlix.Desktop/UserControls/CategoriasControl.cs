using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;
using SenacFlix.Desktop.Sessao;

namespace SenacFlix.Desktop.UserControls
{
    public partial class CategoriasControl : UserControl
    {
        private CategoriaApiCliente _categoriaCliente;

        public CategoriasControl()
        {
            InitializeComponent();
            _categoriaCliente = new CategoriaApiCliente();
        }

        private async void CategoriasControl_Load(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
            ConfigurarPermissoes();
        }

        private void ConfigurarPermissoes()
        {
            // Apenas Admin pode excluir
            btnExcluir.Visible = SessaoUsuario.Instancia.EhAdmin;
        }

        private async Task CarregarDadosAsync(string pesquisa = "")
        {
            try
            {
                var categorias = await _categoriaCliente.ObterTodasAsync();
                
                // Filtro local simples caso a API não tenha BuscarAsync
                if (!string.IsNullOrWhiteSpace(pesquisa) && categorias != null)
                {
                    categorias = categorias.FindAll(c => c.Nome.IndexOf(pesquisa, StringComparison.OrdinalIgnoreCase) >= 0);
                }

                dgvCategorias.DataSource = categorias;
                
                if (dgvCategorias.Columns.Contains("Id"))
                    dgvCategorias.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar categorias: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var form = new SenacFlix.Desktop.Forms.FormCategoria();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CarregarDadosAsync(txtPesquisa.Text);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count > 0)
            {
                var id = (int)dgvCategorias.SelectedRows[0].Cells["Id"].Value;
                var form = new SenacFlix.Desktop.Forms.FormCategoria(id);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _ = CarregarDadosAsync(txtPesquisa.Text);
                }
            }
            else
            {
                MessageBox.Show("Selecione uma categoria para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvCategorias.SelectedRows.Count > 0)
            {
                var id = (int)dgvCategorias.SelectedRows[0].Cells["Id"].Value;
                var nome = dgvCategorias.SelectedRows[0].Cells["Nome"].Value.ToString();
                
                var result = MessageBox.Show($"Deseja realmente excluir a categoria '{nome}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var response = await _categoriaCliente.DesativarAsync(id);
                        if (response.Sucesso)
                        {
                            MessageBox.Show("Categoria excluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await CarregarDadosAsync(txtPesquisa.Text);
                        }
                        else
                        {
                            MessageBox.Show($"Erro ao excluir: {response.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Selecione uma categoria para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
