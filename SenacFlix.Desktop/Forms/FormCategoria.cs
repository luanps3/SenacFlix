using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;

namespace SenacFlix.Desktop.Forms
{
    public partial class FormCategoria : Form
    {
        private CategoriaApiCliente _categoriaCliente;
        private int? _categoriaId;

        public FormCategoria(int? categoriaId = null)
        {
            InitializeComponent();
            _categoriaCliente = new CategoriaApiCliente();
            _categoriaId = categoriaId;
        }

        private async void FormCategoria_Load(object sender, EventArgs e)
        {
            if (_categoriaId.HasValue)
            {
                lblTitle.Text = "Editar Categoria";
                await CarregarCategoriaAsync(_categoriaId.Value);
            }
            else
            {
                lblTitle.Text = "Nova Categoria";
            }
        }

        private async Task CarregarCategoriaAsync(int id)
        {
            try
            {
                var categoria = await _categoriaCliente.ObterPorIdAsync(id);
                if (categoria != null)
                {
                    txtNome.Text = categoria.Nome;
                    txtDescricao.Text = categoria.Descricao;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar categoria: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome é obrigatório.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new CategoriaDto
            {
                Nome = txtNome.Text,
                Descricao = txtDescricao.Text,
                Ativo = true
            };

            try
            {
                if (_categoriaId.HasValue)
                {
                    dto.Id = _categoriaId.Value;
                    var response = await _categoriaCliente.AtualizarAsync(_categoriaId.Value, dto);
                    if (response.Sucesso)
                    {
                        MessageBox.Show("Categoria atualizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    var response = await _categoriaCliente.CadastrarAsync(dto);
                    if (response.Sucesso)
                    {
                        MessageBox.Show("Categoria cadastrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
