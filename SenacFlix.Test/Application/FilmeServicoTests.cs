/*
 * ============================================================
 * Arquivo:   FilmeServicoTests.cs
 * Camada:    SenacFlix.Test / Application
 * Finalidade:
 *   Testes automatizados do servico FilmeServico.
 *
 * O que esta sendo testado:
 *   - ObterTodosAsync: retorna lista de filmes.
 *   - ObterPorIdAsync: retorna filme ou falha quando nao encontrado.
 *   - CadastrarAsync: valida categoria antes de cadastrar.
 *   - AtualizarAsync: valida Id e existencia do filme.
 *   - DesativarAsync: valida existencia antes de desativar.
 *   - ReativarAsync: valida existencia antes de reativar.
 *
 * Conceitos demonstrados:
 *   [Fact]     = teste com cenario fixo.
 *   Mock       = objeto falso que substitui uma dependencia real.
 *   Setup      = configura o comportamento do Mock.
 *   Verify     = verifica se um metodo do Mock foi chamado.
 *   Triple AAA = Arrange, Act, Assert.
 *
 * O QUE E UM MOCK?
 *   Um Mock e um objeto falso utilizado durante o teste para
 *   substituir uma dependencia real. Por exemplo, em vez de
 *   acessar o banco de dados real, criamos um Mock do repositorio
 *   que simula as respostas esperadas.
 *
 *   Vantagens:
 *   - Testes mais rapidos (sem acesso ao banco).
 *   - Testes isolados (nao dependem de infraestrutura).
 *   - Podemos simular qualquer cenario (sucesso, falha, excepcao).
 * ============================================================
 */

using AutoMapper;
using Moq;
using SenacFlix.Application.DTOs;
using SenacFlix.Application.Servicos.Implementacoes;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Application
{
    /// <summary>
    /// Classe de testes do servico FilmeServico.
    /// Utiliza Mocks para simular repositorios e AutoMapper.
    /// </summary>
    public class FilmeServicoTests
    {
        // --------------------------------------------------------
        // DECLARACAO DOS MOCKS
        // --------------------------------------------------------
        // Cada Mock simula uma dependencia do FilmeServico.
        // O FilmeServico depende de: IFilmeRepositorio, ICategoriaRepositorio e IMapper.

        private readonly Mock<IFilmeRepositorio> _filmeRepositorioMock;
        private readonly Mock<ICategoriaRepositorio> _categoriaRepositorioMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FilmeServico _servico;

        /// <summary>
        /// Construtor da classe de testes.
        /// O xUnit cria uma nova instancia desta classe para cada teste,
        /// garantindo que os mocks estejam "limpos" a cada execucao.
        /// </summary>
        public FilmeServicoTests()
        {
            // Criamos os objetos Mock.
            // Mock<T> cria um objeto falso que implementa a interface T.
            _filmeRepositorioMock = new Mock<IFilmeRepositorio>();
            _categoriaRepositorioMock = new Mock<ICategoriaRepositorio>();
            _mapperMock = new Mock<IMapper>();

            // Criamos o servico real, mas injetamos os Mocks como dependencias.
            // Isso permite que o servico funcione normalmente, mas sem acessar
            // o banco de dados real.
            _servico = new FilmeServico(
                _filmeRepositorioMock.Object,    // .Object retorna a implementacao falsa
                _categoriaRepositorioMock.Object,
                _mapperMock.Object
            );
        }

        // =====================================================
        // TESTES POSITIVOS
        // =====================================================

        /// <summary>
        /// Verifica que ObterTodosAsync retorna uma lista de filmes com sucesso.
        /// </summary>
        [Fact]
        public async Task DeveObterTodosOsFilmesComSucesso()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Criamos uma lista de filmes que o Mock ira retornar.
            var filmes = new List<Filme> { TestDataHelper.CriarFilmeValido() };
            var filmeDtos = new List<FilmeDto> { new FilmeDto { Id = 1, Titulo = "Interestelar" } };

            // Setup: configuramos o Mock para retornar a lista quando ObterTodosAsync for chamado.
            // Isso significa: "quando alguem chamar ObterTodosAsync(false), retorne a lista de filmes".
            _filmeRepositorioMock
                .Setup(repo => repo.ObterTodosAsync(false))
                .ReturnsAsync(filmes);

            // Configuramos o Mapper para converter Filmes em FilmeDtos.
            _mapperMock
                .Setup(m => m.Map<IEnumerable<FilmeDto>>(filmes))
                .Returns(filmeDtos);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterTodosAsync();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso, "A operacao deve retornar sucesso.");
            Assert.NotNull(resultado.Dados);

            // Verify: verificamos que o metodo ObterTodosAsync do repositorio foi chamado
            // exatamente UMA VEZ. Isso garante que o servico realmente acessou o repositorio.
            _filmeRepositorioMock.Verify(
                repo => repo.ObterTodosAsync(false),
                Times.Once,
                "O repositorio deve ser chamado exatamente uma vez."
            );
        }

        /// <summary>
        /// Verifica que ObterPorIdAsync retorna o filme quando encontrado.
        /// </summary>
        [Fact]
        public async Task DeveObterFilmePorIdQuandoEncontrado()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var filme = TestDataHelper.CriarFilmeValido();
            var filmeDto = new FilmeDto { Id = 1, Titulo = "Interestelar" };

            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync(filme);

            _mapperMock
                .Setup(m => m.Map<FilmeDto>(filme))
                .Returns(filmeDto);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterPorIdAsync(1);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
            Assert.Equal("Interestelar", resultado.Dados!.Titulo);
        }

        /// <summary>
        /// Verifica que CadastrarAsync cria o filme quando a categoria e valida.
        /// </summary>
        [Fact]
        public async Task DeveCadastrarFilmeQuandoCategoriaValida()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = TestDataHelper.CriarFilmeDtoValido();
            var categoria = TestDataHelper.CriarCategoriaValida();
            var filme = TestDataHelper.CriarFilmeValido();
            var filmeDto = new FilmeDto { Id = 1, Titulo = "Duna: Parte Dois" };

            // A categoria existe no repositorio.
            _categoriaRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(dto.CategoriaId))
                .ReturnsAsync(categoria);

            // O mapper converte o DTO para entidade.
            _mapperMock
                .Setup(m => m.Map<Filme>(dto))
                .Returns(filme);

            // O repositorio adiciona e retorna o filme.
            _filmeRepositorioMock
                .Setup(repo => repo.AdicionarAsync(filme))
                .ReturnsAsync(filme);

            // Busca novamente para carregar relacionamentos.
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(filme.Id))
                .ReturnsAsync(filme);

            // O mapper converte a entidade para DTO de retorno.
            _mapperMock
                .Setup(m => m.Map<FilmeDto>(filme))
                .Returns(filmeDto);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.CadastrarAsync(dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso, "Cadastro com categoria valida deve retornar sucesso.");

            // Verificamos que o filme foi realmente adicionado ao repositorio.
            _filmeRepositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Filme>()),
                Times.Once,
                "O metodo AdicionarAsync deve ser chamado uma vez."
            );
        }

        /// <summary>
        /// Verifica que DesativarAsync retorna sucesso quando o filme existe.
        /// </summary>
        [Fact]
        public async Task DeveDesativarFilmeQuandoExistente()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var filme = TestDataHelper.CriarFilmeValido();

            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync(filme);

            _filmeRepositorioMock
                .Setup(repo => repo.DesativarAsync(1))
                .Returns(Task.CompletedTask);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.DesativarAsync(1);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);

            _filmeRepositorioMock.Verify(
                repo => repo.DesativarAsync(1),
                Times.Once
            );
        }

        // =====================================================
        // TESTES NEGATIVOS — Cenarios de Erro
        // =====================================================

        /// <summary>
        /// Quando o filme nao e encontrado, ObterPorIdAsync deve retornar falha.
        /// Este e um teste NEGATIVO: verificamos como o sistema reage
        /// a uma situacao invalida (filme inexistente).
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeNaoEncontrado()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Configuramos o Mock para retornar null (filme nao existe).
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Filme?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterPorIdAsync(999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso, "Deve retornar falha quando filme nao existe.");
            Assert.Equal("Filme nao encontrado.", resultado.Mensagem);
        }

        /// <summary>
        /// Quando a categoria informada nao existe, o cadastro deve falhar.
        /// Esta e uma regra de negocio real: todo filme deve pertencer
        /// a uma categoria valida.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoCategoriaInvalidaNoCadastro()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = TestDataHelper.CriarFilmeDtoValido();

            // A categoria NAO existe no repositorio (retorna null).
            _categoriaRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(dto.CategoriaId))
                .ReturnsAsync((Categoria?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.CadastrarAsync(dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso, "Cadastro com categoria invalida deve falhar.");
            Assert.Equal("Categoria invalida.", resultado.Mensagem);

            // O filme NAO deve ser adicionado ao repositorio.
            _filmeRepositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Filme>()),
                Times.Never,
                "Nenhum filme deve ser adicionado quando a categoria e invalida."
            );
        }

        /// <summary>
        /// Quando o Id na URL e diferente do Id no corpo da requisicao,
        /// a atualizacao deve falhar. Regra de seguranca.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoIdDivergenteNaAtualizacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = new AtualizarFilmeDto
            {
                Id = 5, // Id no corpo da requisicao
                Titulo = "Filme Editado",
                Descricao = "Descricao editada.",
                AnoLancamento = 2024,
                Duracao = 120,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            // ==========================================
            // ACT
            // ==========================================
            // O Id na URL (10) e diferente do Id no DTO (5).
            var resultado = await _servico.AtualizarAsync(10, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("O Id informado na URL e diferente do Id no corpo da requisicao.", resultado.Mensagem);
        }

        /// <summary>
        /// Quando o filme nao existe, a atualizacao deve falhar.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeNaoExisteNaAtualizacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = new AtualizarFilmeDto
            {
                Id = 999,
                Titulo = "Filme Inexistente",
                Descricao = "Descricao.",
                AnoLancamento = 2024,
                Duracao = 120,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Filme?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AtualizarAsync(999, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Filme nao encontrado.", resultado.Mensagem);
        }

        /// <summary>
        /// Quando o filme nao e encontrado, a desativacao deve falhar.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeNaoExisteNaDesativacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Filme?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.DesativarAsync(999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Filme nao encontrado.", resultado.Mensagem);
        }

        /// <summary>
        /// Quando o filme nao e encontrado, a reativacao deve falhar.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeNaoExisteNaReativacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Filme?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ReativarAsync(999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Filme nao encontrado.", resultado.Mensagem);
        }
    }
}
