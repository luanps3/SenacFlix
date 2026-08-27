/*
 * ============================================================
 * Arquivo:   CategoriaTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da entidade Categoria do dominio SenacFlix.
 *
 * O que esta sendo testado:
 *   - Criacao de uma Categoria com dados validos.
 *   - Valor padrao da propriedade Ativo (true).
 *   - Inicializacao da colecao de Filmes.
 *   - Atributo MaxLength no campo Nome.
 *
 * Conceitos demonstrados:
 *   [Fact]       = teste com cenario unico e dados fixos.
 *   [Theory]     = teste parametrizado com varias entradas.
 *   [InlineData] = fornece dados para o Theory.
 *   Triple AAA   = Arrange, Act, Assert.
 * ============================================================
 */

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SenacFlix.Domain.Entidades;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da entidade Categoria.
    /// </summary>
    public class CategoriaTests
    {
        // =====================================================
        // TESTES POSITIVOS
        // =====================================================

        /// <summary>
        /// Verifica se uma Categoria pode ser criada com dados validos.
        /// </summary>
        [Fact]
        public void DeveCriarCategoriaComDadosValidos()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            // Usamos o helper para criar uma categoria valida.
            var categoria = TestDataHelper.CriarCategoriaValida();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.NotNull(categoria);
            Assert.Equal("Ficcao Cientifica", categoria.Nome);
            Assert.Equal("Filmes de ficcao cientifica e espaco.", categoria.Descricao);
            Assert.True(categoria.Ativo);
        }

        /// <summary>
        /// Toda categoria criada deve ser ativa por padrao.
        /// Isso garante que novas categorias aparecam imediatamente na plataforma.
        /// </summary>
        [Fact]
        public void CategoriaDeveSerAtivaPorPadrao()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var categoria = new Categoria
            {
                Nome = "Terror"
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(categoria.Ativo, "Toda categoria deve ser ativa por padrao.");
        }

        /// <summary>
        /// A colecao de Filmes deve ser inicializada como lista vazia.
        /// Isso evita NullReferenceException ao iterar sobre os filmes da categoria.
        /// </summary>
        [Fact]
        public void CategoriaDevePossuirColecaoDeFilmesInicializada()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var categoria = new Categoria
            {
                Nome = "Comedia"
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.NotNull(categoria.Filmes);
            Assert.Empty(categoria.Filmes);
        }

        /// <summary>
        /// A descricao da categoria e opcional (nullable).
        /// </summary>
        [Fact]
        public void CategoriaDevePermitirDescricaoNula()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var categoria = new Categoria
            {
                Nome = "Drama"
                // Descricao nao preenchida
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Null(categoria.Descricao);
        }

        // =====================================================
        // TESTES COM [THEORY]
        // =====================================================

        /// <summary>
        /// Verifica que o atributo MaxLength(100) esta presente no Nome.
        /// 
        /// Utilizamos [Theory] aqui para demonstrar que a mesma verificacao
        /// poderia ser aplicada a diferentes propriedades. Neste caso temos
        /// apenas uma propriedade com MaxLength na Categoria.
        /// </summary>
        [Theory]
        [InlineData("Nome", 100)]
        public void PropriedadeDeveTerMaxLengthCorreto(string nomePropriedade, int maxLengthEsperado)
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var propriedade = typeof(Categoria).GetProperty(nomePropriedade);

            // ==========================================
            // ACT
            // ==========================================
            var atributo = propriedade?.GetCustomAttribute<MaxLengthAttribute>();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.NotNull(atributo);
            Assert.Equal(maxLengthEsperado, atributo!.Length);
        }

        /// <summary>
        /// Demonstra que diferentes nomes validos podem ser usados para criar categorias.
        /// 
        /// [Theory] com [InlineData] e mais adequado aqui do que criar
        /// um [Fact] separado para cada nome, pois a logica do teste e identica.
        /// </summary>
        [Theory]
        [InlineData("Acao")]
        [InlineData("Romance")]
        [InlineData("Documentario")]
        [InlineData("Animacao")]
        public void CategoriaDeveAceitarDiferentesNomesValidos(string nome)
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var categoria = new Categoria
            {
                Nome = nome
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(nome, categoria.Nome);
        }
    }
}
