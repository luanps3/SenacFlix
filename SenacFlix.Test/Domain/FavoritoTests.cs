/*
 * ============================================================
 * Arquivo:   FavoritoTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da entidade Favorito do dominio SenacFlix.
 *
 * O que esta sendo testado:
 *   - Criacao de um Favorito com dados validos.
 *   - Registro da data do favorito.
 *   - Relacionamentos com UsuarioId e FilmeId.
 *
 * Conceitos demonstrados:
 *   [Fact]     = teste com dados fixos.
 *   Triple AAA = Arrange, Act, Assert.
 * ============================================================
 */

using SenacFlix.Domain.Entidades;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da entidade Favorito.
    /// Testa a criacao e propriedades do registro de favorito.
    /// </summary>
    public class FavoritoTests
    {
        /// <summary>
        /// Verifica se um Favorito pode ser criado com dados validos.
        /// Um favorito representa a relacao entre um usuario e um filme.
        /// </summary>
        [Fact]
        public void DeveCriarFavoritoComDadosValidos()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            // Criamos um favorito usando o helper com dados validos.
            var favorito = TestDataHelper.CriarFavoritoValido();

            // ==========================================
            // ASSERT
            // ==========================================
            // Verificamos que todas as propriedades foram preenchidas.
            Assert.NotNull(favorito);
            Assert.Equal("usuario-guid-123", favorito.UsuarioId);
            Assert.Equal(1, favorito.FilmeId);
        }

        /// <summary>
        /// A data do favorito deve ser registrada para permitir
        /// ordenacao por data de adicao na lista de favoritos do usuario.
        /// </summary>
        [Fact]
        public void FavoritoDeveRegistrarDataDeFavorito()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Capturamos o momento antes da criacao para comparar depois.
            var antesDoTeste = DateTime.UtcNow;

            // ==========================================
            // ACT
            // ==========================================
            var favorito = new Favorito
            {
                UsuarioId = "usuario-456",
                FilmeId = 2,
                DataFavorito = DateTime.UtcNow
            };

            // ==========================================
            // ASSERT
            // ==========================================
            // A data do favorito deve ser igual ou posterior ao momento antes do teste.
            Assert.True(favorito.DataFavorito >= antesDoTeste,
                "A data do favorito deve ser registrada no momento da criacao.");
        }

        /// <summary>
        /// O UsuarioId e uma string (GUID) porque o ASP.NET Identity
        /// usa string como tipo de chave primaria por padrao.
        /// </summary>
        [Fact]
        public void FavoritoDevePossuirUsuarioIdComoString()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var guidUsuario = Guid.NewGuid().ToString();
            var favorito = new Favorito
            {
                UsuarioId = guidUsuario,
                FilmeId = 1,
                DataFavorito = DateTime.UtcNow
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(guidUsuario, favorito.UsuarioId);
            Assert.IsType<string>(favorito.UsuarioId);
        }
    }
}
