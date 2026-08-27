using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;

namespace SenacFlix.Desktop.Forms
{
    public partial class FormFilme : Form
    {
        private FilmeApiCliente _filmeCliente;
        private int? _filmeId;

        public FormFilme(int? filmeId = null)
        {
            InitializeComponent();
            _filmeCliente = new FilmeApiCliente();
            _filmeId = filmeId;
            
            // Adicionando eventos manuais
            txtUrlCapa.TextChanged += TxtUrlCapa_TextChanged;
            txtUrlBanner.TextChanged += TxtUrlBanner_TextChanged;
            
            btnUploadCapa.Click += BtnUploadCapa_Click;
            btnUploadBanner.Click += BtnUploadBanner_Click;
            
            btnVisualizarTrailer.Click += BtnVisualizarTrailer_Click;
            btnVisualizarFilme.Click += BtnVisualizarFilme_Click;
        }

        private async void FormFilme_Load(object sender, EventArgs e)
        {
            if (_filmeId.HasValue)
            {
                lblTitle.Text = "Editar Filme";
                await CarregarFilmeAsync(_filmeId.Value);
            }
            else
            {
                lblTitle.Text = "Novo Filme";
                chkAtivo.Checked = true;
            }
        }

        private async Task CarregarFilmeAsync(int id)
        {
            try
            {
                var filme = await _filmeCliente.ObterPorIdAsync(id);
                if (filme != null)
                {
                    txtTitulo.Text = filme.Titulo;
                    txtDescricao.Text = filme.Descricao;
                    txtAno.Text = filme.AnoLancamento.ToString();
                    txtDuracao.Text = filme.Duracao.ToString();
                    txtDiretor.Text = filme.Diretor;
                    txtElenco.Text = filme.Elenco;
                    txtCategoriaId.Text = filme.CategoriaId.ToString();
                    txtClassificacaoId.Text = filme.ClassificacaoIndicativaId.ToString();
                    
                    chkAtivo.Checked = filme.Ativo;
                    chkDestaqueHome.Checked = filme.DestaqueHome;
                    
                    txtUrlCapa.Text = filme.ImagemCapaUrl;
                    txtUrlBanner.Text = filme.ImagemBannerUrl;
                    txtTrailerUrl.Text = filme.TrailerYoutubeUrl;
                    txtVideoUrl.Text = filme.VideoYoutubeUrl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar filme: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validacoes
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("O título é obrigatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(txtTrailerUrl.Text))
            {
                MessageBox.Show("O Trailer é obrigatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(txtVideoUrl.Text))
            {
                MessageBox.Show("A URL do Filme é obrigatória.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new FilmeDto
            {
                Titulo = txtTitulo.Text,
                Descricao = txtDescricao.Text,
                Diretor = string.IsNullOrWhiteSpace(txtDiretor.Text) ? null : txtDiretor.Text,
                Elenco = string.IsNullOrWhiteSpace(txtElenco.Text) ? null : txtElenco.Text,
                Ativo = chkAtivo.Checked,
                DestaqueHome = chkDestaqueHome.Checked,
                ImagemCapaUrl = string.IsNullOrWhiteSpace(txtUrlCapa.Text) ? null : txtUrlCapa.Text,
                ImagemBannerUrl = string.IsNullOrWhiteSpace(txtUrlBanner.Text) ? null : txtUrlBanner.Text,
                TrailerYoutubeUrl = txtTrailerUrl.Text,
                VideoYoutubeUrl = txtVideoUrl.Text
            };

            int.TryParse(txtAno.Text, out int ano);
            dto.AnoLancamento = ano;

            int.TryParse(txtDuracao.Text, out int duracao);
            dto.Duracao = duracao;

            int.TryParse(txtCategoriaId.Text, out int catId);
            dto.CategoriaId = catId;

            int.TryParse(txtClassificacaoId.Text, out int classId);
            dto.ClassificacaoIndicativaId = classId;

            try
            {
                if (_filmeId.HasValue)
                {
                    dto.Id = _filmeId.Value;
                    var response = await _filmeCliente.AtualizarAsync(_filmeId.Value, dto);
                    if (response.Sucesso)
                    {
                        MessageBox.Show("Filme atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Erro ao atualizar: {response.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var response = await _filmeCliente.CadastrarAsync(dto);
                    if (response.Sucesso)
                    {
                        MessageBox.Show("Filme cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Erro ao cadastrar: {response.Mensagem}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na requisição: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        
        // --- Lógica Didática de Imagens e Links ---

        private void TxtUrlCapa_TextChanged(object sender, EventArgs e)
        {
            CarregarPreviewImagem(txtUrlCapa.Text, picPreviewCapa);
        }

        private void TxtUrlBanner_TextChanged(object sender, EventArgs e)
        {
            CarregarPreviewImagem(txtUrlBanner.Text, picPreviewBanner);
        }

        private void CarregarPreviewImagem(string url, PictureBox picBox)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                picBox.Image = null;
                return;
            }

            try
            {
                if (url.StartsWith("/"))
                {
                    // Trata URL relativa da API web local
                    url = $"http://localhost:5260{url}";
                }

                picBox.LoadAsync(url);
            }
            catch
            {
                // Ignora erro de preview se a URL for invalida
                picBox.Image = null;
            }
        }

        private void BtnUploadCapa_Click(object sender, EventArgs e)
        {
            RealizarUploadLocal(txtUrlCapa);
        }

        private void BtnUploadBanner_Click(object sender, EventArgs e)
        {
            RealizarUploadLocal(txtUrlBanner);
        }

        private void RealizarUploadLocal(Guna.UI2.WinForms.Guna2TextBox textBoxUrl)
        {
            // Implementação 100% didática para copiar a imagem diretamente 
            // para a pasta wwwroot da Web Api/UI, mantendo a arquitetura simples
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.webp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "..", "SenacFlix.UI", "wwwroot", "uploads", "filmes");
                        if (!Directory.Exists(pastaDestino))
                        {
                            Directory.CreateDirectory(pastaDestino);
                        }

                        string nomeArquivo = $"{Guid.NewGuid()}{Path.GetExtension(ofd.FileName)}";
                        string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

                        File.Copy(ofd.FileName, caminhoCompleto, true);

                        // O caminho armazenado no banco será o caminho relativo a partir do wwwroot da aplicação web
                        textBoxUrl.Text = $"/uploads/filmes/{nomeArquivo}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erro ao salvar imagem localmente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnVisualizarTrailer_Click(object sender, EventArgs e)
        {
            AbrirLink(txtTrailerUrl.Text);
        }

        private void BtnVisualizarFilme_Click(object sender, EventArgs e)
        {
            AbrirLink(txtVideoUrl.Text);
        }

        private void AbrirLink(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                // Process.Start no .NET Core / 5+ requer UseShellExecute = true para abrir URLs
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível abrir o link. Verifique se a URL está correta.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
