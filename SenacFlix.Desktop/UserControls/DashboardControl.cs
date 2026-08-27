using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SenacFlix.Desktop.ApiClientes;

namespace SenacFlix.Desktop.UserControls
{
    public partial class DashboardControl : UserControl
    {
        private FilmeApiCliente _filmeCliente;
        private CategoriaApiCliente _categoriaCliente;
        private UsuarioApiCliente _usuarioCliente;

        public DashboardControl()
        {
            InitializeComponent();
            _filmeCliente = new FilmeApiCliente();
            _categoriaCliente = new CategoriaApiCliente();
            _usuarioCliente = new UsuarioApiCliente();
        }

        private async void DashboardControl_Load(object sender, EventArgs e)
        {
            await CarregarDadosAsync();
        }

        private async Task CarregarDadosAsync()
        {
            try
            {
                var filmes = await _filmeCliente.ObterTodosAsync();
                if (filmes != null)
                {
                    lblTotalFilmes.Text = filmes.Count.ToString();
                }

                var categorias = await _categoriaCliente.ObterTodasAsync();
                if (categorias != null)
                {
                    lblTotalCategorias.Text = categorias.Count.ToString();
                }

                var usuarios = await _usuarioCliente.ObterTodosAsync();
                if (usuarios != null)
                {
                    lblTotalUsuarios.Text = usuarios.Count.ToString();
                }
            }
            catch
            {
                // Em caso de erro, os labels ficam com "..."
            }
        }
    }
}
