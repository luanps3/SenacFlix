/*
 * ============================================================
 * Arquivo:   CategoriaServicoTests.cs
 * Camada:    SenacFlix.Test / Application
 * Finalidade:
 *   Testes automatizados do servico CategoriaServico.
 *
 * O que esta sendo testado:
 *   - ObterTodasAsync: retorna lista de categorias.
 *   - ObterPorIdAsync: retorna categoria ou falha quando nao encontrada.
 *   - CadastrarAsync: impede cadastro de nome duplicado (case-insensitive).
 *   - AtualizarAsync: impede atualizacao com nome duplicado de outra categoria.
 *   - ExcluirPermanentementeAsync: impede exclusao quando ha filmes vinculados.
 *   - DesativarAsync: valida existencia antes de desativar.
 *
 * Conceitos demonstrados:
 *   Mock com Moq   = simulacao de repositorios.
 *   Verify         = verificacao de chamadas ao repositorio.
 *   Testes negativos = cenarios de erro (duplicidade, inexistencia).
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
    /// Classe de testes do servico CategoriaServico.
    /// Demonstra testes de regras de negocio reais:
    /// - Duplicidade de nome.
    /// - Impedir exclusao com filmes vinculados.
    /// </summary>
    public class CategoriaServicoTests
    {
        private readonly Mock<ICategoriaRepositorio> _repositorioMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CategoriaServico _servico;

        public CategoriaServicoTests()
        {
            _repositorioMock = new Mock<ICategoriaRepositorio>();
            _mapperMock = new Mock<IMapper>();
            _servico = new CategoriaServico(_repositorioMock.Object, _mapperMock.Object);
        }

        // =====================================================
        // TESTES POSITIVOS
        // =====================================================

        /// <summary>
        /// Verifica que ObterTodasAsync retorna lista com sucesso.
        /// </summary>
        [Fact]
        public async Task DeveObterTodasAsCategoriasComSucesso()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var categorias = new List<Categoria> { TestDataHelper.CriarCategoriaValida() };
            var categoriaDtos = new List<CategoriaDto> { new CategoriaDto { Id = 1, Nome = "Ficcao Cientifica" } };

            _repositorioMock
                .Setup(repo => repo.ObterTodasAsync(false))
                .ReturnsAsync(categorias);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<CategoriaDto>>(categorias))
                .Returns(categoriaDtos);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterTodasAsync();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
        }

        /// <summary>
        /// Verifica que o cadastro funciona quando o nome nao e duplicado.
        /// </summary>
        [Fact]
        public async Task DeveCadastrarCategoriaQuandoNomeUnico()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = TestDataHelper.CriarCategoriaDtoValido();
            var categoriaExistente = new Categoria { Id = 1, Nome = "Terror" }; // Nome diferente
            var categoriaCriada = new Categoria { Id = 2, Nome = "Acao" };
            var categoriaDto = new CategoriaDto { Id = 2, Nome = "Acao" };

            // Retorna lista com UMA categoria existente (nome diferente do DTO).
            _repositorioMock
                .Setup(repo => repo.ObterTodasAsync(true))
                .ReturnsAsync(new List<Categoria> { categoriaExistente });

            _mapperMock
                .Setup(m => m.Map<Categoria>(dto))
                .Returns(categoriaCriada);

            _repositorioMock
                .Setup(repo => repo.AdicionarAsync(categoriaCriada))
                .ReturnsAsync(categoriaCriada);

            _mapperMock
                .Setup(m => m.Map<CategoriaDto>(categoriaCriada))
                .Returns(categoriaDto);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.CadastrarAsync(dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso, "Cadastro com nome unico deve retornar sucesso.");

            _repositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Categoria>()),
                Times.Once
            );
        }

        // =====================================================
        // TESTES NEGATIVOS
        // =====================================================

        /// <summary>
        /// Quando a categoria nao e encontrada, ObterPorIdAsync deve retornar falha.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoCategoriaNaoEncontrada()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _repositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Categoria?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterPorIdAsync(999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Categoria nao encontrada.", resultado.Mensagem);
        }

        /// <summary>
        /// O sistema NAO deve permitir cadastrar duas categorias com o MESMO NOME.
        /// A comparacao e case-insensitive: "ACAO" e "acao" sao considerados iguais.
        /// Esta e uma regra de negocio REAL do CategoriaServico.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoNomeDuplicadoNoCadastro()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = new CriarCategoriaDto { Nome = "Acao" };
            var categoriaExistente = new Categoria { Id = 1, Nome = "Acao" }; // Mesmo nome!

            // O repositorio retorna uma categoria com o mesmo nome.
            _repositorioMock
                .Setup(repo => repo.ObterTodasAsync(true))
                .ReturnsAsync(new List<Categoria> { categoriaExistente });

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.CadastrarAsync(dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso, "Cadastro com nome duplicado deve falhar.");
            Assert.Equal("Já existe uma categoria com este nome.", resultado.Mensagem);

            // O repositorio NAO deve adicionar a categoria duplicada.
            _repositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Categoria>()),
                Times.Never,
                "Nenhuma categoria deve ser adicionada quando o nome e duplicado."
            );
        }

        /// <summary>
        /// Na atualizacao, impede que o novo nome seja igual ao de OUTRA categoria.
        /// A propria categoria pode manter seu nome (Id diferente e o que conta).
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoNomeDuplicadoDeOutraNaAtualizacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = new CriarCategoriaDto { Nome = "Terror" };
            var categoriaAtual = new Categoria { Id = 1, Nome = "Acao" };
            var outraCategoria = new Categoria { Id = 2, Nome = "Terror" }; // Outra com mesmo nome

            _repositorioMock
                .Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync(categoriaAtual);

            _repositorioMock
                .Setup(repo => repo.ObterTodasAsync(true))
                .ReturnsAsync(new List<Categoria> { categoriaAtual, outraCategoria });

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AtualizarAsync(1, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Já existe outra categoria com este nome.", resultado.Mensagem);
        }

        /// <summary>
        /// Quando a categoria nao e encontrada, a atualizacao deve falhar.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoCategoriaNaoExisteNaAtualizacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dto = new CriarCategoriaDto { Nome = "Nova Categoria" };

            _repositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Categoria?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AtualizarAsync(999, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Categoria nao encontrada.", resultado.Mensagem);
        }

        /// <summary>
        /// NAO deve ser possivel excluir permanentemente uma categoria
        /// que possui filmes vinculados. Esta e uma regra de integridade.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoExcluirCategoriaComFilmesVinculados()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var categoria = TestDataHelper.CriarCategoriaValida();

            // Adicionamos um filme vinculado a esta categoria.
            categoria.Filmes = new List<Filme>
            {
                new Filme
                {
                    Id = 1,
                    Titulo = "Filme Vinculado",
                    Descricao = "Este filme pertence a categoria.",
                    AnoLancamento = 2024,
                    Duracao = 120,
                    CategoriaId = categoria.Id,
                    ClassificacaoIndicativaId = 1,
                    Ativo = true
                }
            };

            _repositorioMock
                .Setup(repo => repo.ObterPorIdAsync(categoria.Id))
                .ReturnsAsync(categoria);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ExcluirPermanentementeAsync(categoria.Id);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal(
                "Não é possível excluir uma categoria que esteja sendo utilizada por algum filme.",
                resultado.Mensagem
            );

            // ExcluirPermanentemente NAO deve ser chamado.
            _repositorioMock.Verify(
                repo => repo.ExcluirPermanentementeAsync(It.IsAny<int>()),
                Times.Never
            );
        }

        /// <summary>
        /// Quando a categoria nao e encontrada, a desativacao deve falhar.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoCategoriaNaoExisteNaDesativacao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _repositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Categoria?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.DesativarAsync(999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Categoria nao encontrada.", resultado.Mensagem);
        }
    }
}
