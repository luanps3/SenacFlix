/*
 * ============================================================
 * Arquivo:   FavoritoServicoTests.cs
 * Camada:    SenacFlix.Test / Application
 * Finalidade:
 *   Testes automatizados do servico FavoritoServico.
 *
 * O que esta sendo testado:
 *   - ObterFavoritosDoUsuarioAsync: retorna lista de favoritos.
 *   - AdicionarFavoritoAsync: valida filme, impede duplicidade.
 *   - RemoverFavoritoAsync: valida existencia do favorito.
 *   - VerificarFavoritoAsync: retorna se filme e favorito.
 *
 * Conceitos demonstrados:
 *   Mock       = simulacao de IFavoritoRepositorio e IFilmeRepositorio.
 *   Verify     = verificacao de chamadas.
 *   Testes de duplicidade = regra real de impedir favoritos duplicados.
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
    /// Classe de testes do servico FavoritoServico.
    /// Testa as regras de favoritar filmes.
    /// </summary>
    public class FavoritoServicoTests
    {
        private readonly Mock<IFavoritoRepositorio> _favoritoRepositorioMock;
        private readonly Mock<IFilmeRepositorio> _filmeRepositorioMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FavoritoServico _servico;

        public FavoritoServicoTests()
        {
            _favoritoRepositorioMock = new Mock<IFavoritoRepositorio>();
            _filmeRepositorioMock = new Mock<IFilmeRepositorio>();
            _mapperMock = new Mock<IMapper>();

            _servico = new FavoritoServico(
                _favoritoRepositorioMock.Object,
                _filmeRepositorioMock.Object,
                _mapperMock.Object
            );
        }

        // =====================================================
        // TESTES POSITIVOS
        // =====================================================

        /// <summary>
        /// Verifica que ObterFavoritosDoUsuarioAsync retorna lista com sucesso.
        /// </summary>
        [Fact]
        public async Task DeveObterFavoritosDoUsuarioComSucesso()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var usuarioId = "usuario-guid-123";
            var favoritos = new List<Favorito> { TestDataHelper.CriarFavoritoValido() };
            var favoritoDtos = new List<FavoritoDto>
            {
                new FavoritoDto { Id = 1, FilmeId = 1, UsuarioId = usuarioId }
            };

            _favoritoRepositorioMock
                .Setup(repo => repo.ObterPorUsuarioAsync(usuarioId))
                .ReturnsAsync(favoritos);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<FavoritoDto>>(favoritos))
                .Returns(favoritoDtos);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.ObterFavoritosDoUsuarioAsync(usuarioId);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);
            Assert.NotNull(resultado.Dados);
        }

        /// <summary>
        /// Verifica que o favorito e adicionado com sucesso quando
        /// o filme existe e ainda nao foi favoritado.
        /// </summary>
        [Fact]
        public async Task DeveAdicionarFavoritoQuandoFilmeExisteENaoFoiFavoritado()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var usuarioId = "usuario-guid-123";
            var dto = TestDataHelper.CriarAdicionarFavoritoDtoValido();
            var filme = TestDataHelper.CriarFilmeValido();
            var favorito = TestDataHelper.CriarFavoritoValido();
            var favoritoDto = new FavoritoDto { Id = 1, FilmeId = 1, UsuarioId = usuarioId };

            // O filme existe.
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(dto.FilmeId))
                .ReturnsAsync(filme);

            // O filme NAO foi favoritado ainda (false).
            _favoritoRepositorioMock
                .Setup(repo => repo.ExisteAsync(usuarioId, dto.FilmeId))
                .ReturnsAsync(false);

            // O favorito e adicionado com sucesso.
            _favoritoRepositorioMock
                .Setup(repo => repo.AdicionarAsync(It.IsAny<Favorito>()))
                .ReturnsAsync(favorito);

            // Busca completa para o DTO.
            _favoritoRepositorioMock
                .Setup(repo => repo.ObterAsync(usuarioId, dto.FilmeId))
                .ReturnsAsync(favorito);

            _mapperMock
                .Setup(m => m.Map<FavoritoDto>(favorito))
                .Returns(favoritoDto);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AdicionarFavoritoAsync(usuarioId, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso, "Deve adicionar favorito com sucesso.");

            // Verificamos que o favorito foi realmente adicionado.
            _favoritoRepositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Favorito>()),
                Times.Once,
                "AdicionarAsync deve ser chamado uma vez."
            );
        }

        /// <summary>
        /// Verifica que o favorito e removido com sucesso quando encontrado.
        /// </summary>
        [Fact]
        public async Task DeveRemoverFavoritoQuandoExistente()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var usuarioId = "usuario-guid-123";
            var filmeId = 1;
            var favorito = TestDataHelper.CriarFavoritoValido();

            _favoritoRepositorioMock
                .Setup(repo => repo.ObterAsync(usuarioId, filmeId))
                .ReturnsAsync(favorito);

            _favoritoRepositorioMock
                .Setup(repo => repo.RemoverAsync(favorito))
                .Returns(Task.CompletedTask);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.RemoverFavoritoAsync(usuarioId, filmeId);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);

            _favoritoRepositorioMock.Verify(
                repo => repo.RemoverAsync(favorito),
                Times.Once
            );
        }

        /// <summary>
        /// Verifica que VerificarFavoritoAsync retorna true quando e favorito.
        /// </summary>
        [Fact]
        public async Task DeveRetornarTrueQuandoFilmeEFavorito()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _favoritoRepositorioMock
                .Setup(repo => repo.ExisteAsync("usuario-123", 1))
                .ReturnsAsync(true);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.VerificarFavoritoAsync("usuario-123", 1);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resultado.Sucesso);
            Assert.True(resultado.Dados);
        }

        // =====================================================
        // TESTES NEGATIVOS
        // =====================================================

        /// <summary>
        /// NAO deve ser possivel favoritar um filme que nao existe.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeNaoExisteAoFavoritar()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var usuarioId = "usuario-guid-123";
            var dto = new AdicionarFavoritoDto { FilmeId = 999 };

            // O filme NAO existe (retorna null).
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(999))
                .ReturnsAsync((Filme?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AdicionarFavoritoAsync(usuarioId, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso, "Favoritar filme inexistente deve falhar.");
            Assert.Equal("Filme nao encontrado.", resultado.Mensagem);

            // O favorito NAO deve ser adicionado.
            _favoritoRepositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Favorito>()),
                Times.Never
            );
        }

        /// <summary>
        /// NAO deve ser possivel favoritar o mesmo filme duas vezes.
        /// Esta e uma regra de negocio REAL que impede duplicidade.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFilmeJaFoiFavoritado()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var usuarioId = "usuario-guid-123";
            var dto = new AdicionarFavoritoDto { FilmeId = 1 };
            var filme = TestDataHelper.CriarFilmeValido();

            // O filme existe.
            _filmeRepositorioMock
                .Setup(repo => repo.ObterPorIdAsync(1))
                .ReturnsAsync(filme);

            // MAS o usuario JA favoritou este filme (true = ja existe).
            _favoritoRepositorioMock
                .Setup(repo => repo.ExisteAsync(usuarioId, 1))
                .ReturnsAsync(true);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.AdicionarFavoritoAsync(usuarioId, dto);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso, "Favoritar filme duplicado deve falhar.");
            Assert.Equal("Este filme ja esta nos seus favoritos.", resultado.Mensagem);

            // O favorito NAO deve ser adicionado novamente.
            _favoritoRepositorioMock.Verify(
                repo => repo.AdicionarAsync(It.IsAny<Favorito>()),
                Times.Never,
                "AdicionarAsync NAO deve ser chamado quando ja e favorito."
            );
        }

        /// <summary>
        /// NAO deve ser possivel remover um favorito que nao existe.
        /// </summary>
        [Fact]
        public async Task DeveRetornarFalhaQuandoFavoritoNaoExisteNaRemocao()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            _favoritoRepositorioMock
                .Setup(repo => repo.ObterAsync("usuario-123", 999))
                .ReturnsAsync((Favorito?)null);

            // ==========================================
            // ACT
            // ==========================================
            var resultado = await _servico.RemoverFavoritoAsync("usuario-123", 999);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resultado.Sucesso);
            Assert.Equal("Favorito nao encontrado.", resultado.Mensagem);
        }
    }
}
