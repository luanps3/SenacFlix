using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;

namespace SenacFlix.Infrastructure.Repositorios
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly SenacFlixContexto _contexto;

        public CategoriaRepositorio(SenacFlixContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<Categoria>> ObterTodasAsync(bool incluirInativas = false)
        {
            // IQueryable representa uma consulta que pode ser executada contra uma fonte de dados,
            // como um banco de dados. Ele permite construir consultas de forma flexível e eficiente,
            // sem realmente executar a consulta até que seja necessário.
            IQueryable<Categoria> query = _contexto.Categorias
            .Include(c => c.Filmes.Where(f => f.Ativo));

            if (!incluirInativas)
            {
                query = query.Where(c => c.Ativo);
            }

            return await query.ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            // O método Include é usado para carregar a coleção de filmes relacionados à categoria.
            // FirstOrDefaultAsync é usado para obter a primeira categoria que corresponda ao id fornecido ou null se não houver correspondência.
            return await _contexto.Categorias
                .Include(c => c.Filmes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Categoria> AdicionarAsync(Categoria categoria)
        {

            // O método AddAsync é usado para adicionar a categoria ao contexto do Entity Framework,
            // e SaveChangesAsync é chamado para persistir as alterações no banco de dados.
            await _contexto.Categorias.AddAsync(categoria);
            await _contexto.SaveChangesAsync();
            return categoria;
        }

        public async Task AtualizarAsync(Categoria categoria)
        {
            // O método Update é usado para marcar a categoria como modificada no contexto do Entity Framework
            _contexto.Categorias.Update(categoria);
            await _contexto.SaveChangesAsync();
        }

        public async Task DesativarAsync(int id)
        {
            // O método FindAsync é usado para localizar a
            // categoria pelo id fornecido.
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Ativo = false;
                categoria.DataExclusao = DateTime.UtcNow;
                categoria.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }
        public async Task ReativarAsync(int id)
        {
            // O método FindAsync é usado para localizar a
            // categoria pelo id fornecido.
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                categoria.Ativo = true;
                categoria.DataExclusao = null;
                categoria.DataAtualizacao = DateTime.UtcNow;
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task ExcluirPermanentementeAsync(int id)
        {
           
            var categoria = await _contexto.Categorias.FindAsync(id);

            if (categoria != null)
            {
                // O método Remove é usado para marcar a categoria
                // para exclusão no contexto do Entity Framework
                _contexto.Categorias.Remove(categoria);
                await _contexto.SaveChangesAsync();
            }

        }











    }
}
