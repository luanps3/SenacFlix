using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;
using SenacFlix.Desktop.Sessao;

namespace SenacFlix.Desktop.UserControls
{
    public partial class FilmesControl : UserControl
    {
        private FilmeApiCliente _filmeCliente;

        public FilmesControl()
        {
            InitializeComponent();
            _filmeCliente = new FilmeApiCliente();
            dgvFilmes.AutoGenerateColumns = false;
            dgvFilmes.CellFormatting += DgvFilmes_CellFormatting;
        }

        private async void FilmesControl_Load(object sender, EventArgs e)
        {
            // Oculta colunas desnecessárias se houver
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
                var filmes = string.IsNullOrWhiteSpace(pesquisa) ? await _filmeCliente.ObterTodosAsync() : await _filmeCliente.BuscarAsync(pesquisa);

                dgvFilmes.DataSource = filmes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filmes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DgvFilmes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvFilmes.Columns[e.ColumnIndex].Name == "colCapa" && e.RowIndex >= 0)
            {
                var filme = dgvFilmes.Rows[e.RowIndex].DataBoundItem as FilmeDto;
                if (filme != null)
                {
                    // Se a imagem ja foi carregada na celula (nao esta nula e nao e do tipo string), nao precisa carregar de novo
                    if (e.Value != null && e.Value is System.Drawing.Image)
                        return;

                    string url = filme.ImagemCapaUrl;
                    if (!string.IsNullOrEmpty(url))
                    {
                        try
                        {
                            // Apenas define um placeholder enquanto carrega
                            // Em um app real com mtas imagens, um cache em memoria seria ideal
                            using (var client = new System.Net.Http.HttpClient())
                            {
                                // Adiciona localhost caso seja caminho relativo
                                if (url.StartsWith("/"))
                                    url = $"http://localhost:5260{url}"; // Ajuste a porta se necessario, ou HTTPS
                                    
                                var response = await client.GetAsync(url);
                                if (response.IsSuccessStatusCode)
                                {
                                    using (var stream = await response.Content.ReadAsStreamAsync())
                                    {
                                        var image = System.Drawing.Image.FromStream(stream);
                                        dgvFilmes.Rows[e.RowIndex].Cells["colCapa"].Value = image;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            // Falha silenciosa para imagem quebrada
                        }
                    }
                }
            }
        }

        private async void btnAtualizar_Click(object sender, EventArgs e)
        {
            await CarregarDadosAsync(txtPesquisa.Text);
        }

        private async void txtPesquisa_TextChanged(object sender, EventArgs e)
        {
            // Um delayzinho idealmente, mas como é pra ser simples:
            await CarregarDadosAsync(txtPesquisa.Text);
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            var form = new SenacFlix.Desktop.Forms.FormFilme();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _ = CarregarDadosAsync(txtPesquisa.Text);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvFilmes.SelectedRows.Count > 0)
            {
                var id = (int)dgvFilmes.SelectedRows[0].Cells["Id"].Value;
                var form = new SenacFlix.Desktop.Forms.FormFilme(id);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _ = CarregarDadosAsync(txtPesquisa.Text);
                }
            }
            else
            {
                MessageBox.Show("Selecione um filme para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvFilmes.SelectedRows.Count > 0)
            {
                var id = (int)dgvFilmes.SelectedRows[0].Cells["Id"].Value;
                var titulo = dgvFilmes.SelectedRows[0].Cells["Titulo"].Value.ToString();
                
                var result = MessageBox.Show($"Deseja realmente excluir o filme '{titulo}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var response = await _filmeCliente.DesativarAsync(id);
                        if (response.Sucesso)
                        {
                            MessageBox.Show("Filme excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Selecione um filme para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
