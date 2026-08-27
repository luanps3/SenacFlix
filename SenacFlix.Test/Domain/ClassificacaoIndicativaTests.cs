/*
 * ============================================================
 * Arquivo:   ClassificacaoIndicativaTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da entidade ClassificacaoIndicativa.
 *
 * O que esta sendo testado:
 *   - Criacao de uma ClassificacaoIndicativa com dados validos.
 *   - Diferentes idades minimas (Livre, 10+, 12+, 14+, 16+, 18+).
 *   - Atributos MaxLength em Nome e Cor.
 *
 * Conceitos demonstrados:
 *   [Fact]       = teste com cenario unico.
 *   [Theory]     = teste executado com diferentes idades minimas.
 *   [InlineData] = fornece os dados para cada execucao do Theory.
 * ============================================================
 */

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SenacFlix.Domain.Entidades;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da entidade ClassificacaoIndicativa.
    /// </summary>
    public class ClassificacaoIndicativaTests
    {
        /// <summary>
        /// Verifica se uma ClassificacaoIndicativa pode ser criada com dados validos.
        /// </summary>
        [Fact]
        public void DeveCriarClassificacaoComDadosValidos()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var classificacao = TestDataHelper.CriarClassificacaoValida();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.NotNull(classificacao);
            Assert.Equal("12+", classificacao.Nome);
            Assert.Equal(12, classificacao.IdadeMinima);
            Assert.Equal("#F5C518", classificacao.Cor);
        }

        /// <summary>
        /// As classificacoes indicativas brasileiras possuem diferentes idades minimas.
        /// Este teste utiliza [Theory] para verificar que a entidade aceita
        /// todas as idades validas definidas pelo Ministerio da Justica.
        /// 
        /// [Theory] e mais adequado aqui porque estamos testando a mesma regra
        /// (criacao de classificacao) com 6 combinacoes diferentes de dados.
        /// Criar 6 metodos [Fact] seria repetitivo e desnecessario.
        /// </summary>
        [Theory]
        [InlineData("Livre", 0, "#00AA00")]
        [InlineData("10+", 10, "#0099FF")]
        [InlineData("12+", 12, "#F5C518")]
        [InlineData("14+", 14, "#E67E22")]
        [InlineData("16+", 16, "#E74C3C")]
        [InlineData("18+", 18, "#000000")]
        public void DeveAceitarDiferentesIdadesMinimasValidas(string nome, int idadeMinima, string cor)
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var classificacao = new ClassificacaoIndicativa
            {
                Nome = nome,
                IdadeMinima = idadeMinima,
                Cor = cor
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(nome, classificacao.Nome);
            Assert.Equal(idadeMinima, classificacao.IdadeMinima);
            Assert.Equal(cor, classificacao.Cor);
        }

        /// <summary>
        /// Verifica os atributos MaxLength nas propriedades Nome e Cor.
        /// </summary>
        [Theory]
        [InlineData("Nome", 20)]
        [InlineData("Cor", 20)]
        public void PropriedadeDeveTerMaxLengthCorreto(string nomePropriedade, int maxLengthEsperado)
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var propriedade = typeof(ClassificacaoIndicativa).GetProperty(nomePropriedade);

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
        /// A descricao da classificacao e opcional.
        /// </summary>
        [Fact]
        public void ClassificacaoDevePermitirDescricaoNula()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var classificacao = new ClassificacaoIndicativa
            {
                Nome = "Livre",
                IdadeMinima = 0,
                Cor = "#00AA00"
                // Descricao nao preenchida
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Null(classificacao.Descricao);
        }
    }
}
