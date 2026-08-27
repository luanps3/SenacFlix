/*
 * ============================================================
 * Arquivo:   SenacFlixExcecaoTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da classe de excecao SenacFlixExcecao.
 *
 * O que esta sendo testado:
 *   - Construtor padrao (mensagem generica).
 *   - Construtor com mensagem personalizada.
 *   - Construtor com mensagem e inner exception.
 *   - Como testar excecoes utilizando Assert.Throws<T>().
 *
 * Conceitos demonstrados:
 *   [Fact]          = teste com cenario fixo.
 *   Assert.Throws   = como verificar se uma excecao foi lancada.
 *   Triple AAA      = Arrange, Act, Assert.
 *
 * IMPORTANTE:
 *   Assert.Throws<T>() e utilizado para verificar que um bloco de
 *   codigo LANCA uma excecao do tipo T. O teste PASSA quando a
 *   excecao e lancada e FALHA quando ela NAO e lancada.
 * ============================================================
 */

using SenacFlix.Domain.Excecoes;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da excecao personalizada SenacFlixExcecao.
    /// Demonstra como testar excecoes com Assert.Throws.
    /// </summary>
    public class SenacFlixExcecaoTests
    {
        /// <summary>
        /// O construtor padrao deve criar uma excecao com mensagem generica.
        /// </summary>
        [Fact]
        public void ConstrutorPadraoDeveTerMensagemGenerica()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var excecao = new SenacFlixExcecao();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal("Ocorreu um erro na plataforma SenacFlix.", excecao.Message);
        }

        /// <summary>
        /// O construtor com mensagem deve armazenar a mensagem informada.
        /// </summary>
        [Fact]
        public void ConstrutorComMensagemDeveArmazenarMensagem()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var mensagemEsperada = "Filme nao encontrado no catalogo.";

            // ==========================================
            // ACT
            // ==========================================
            var excecao = new SenacFlixExcecao(mensagemEsperada);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(mensagemEsperada, excecao.Message);
        }

        /// <summary>
        /// O construtor com inner exception deve preservar a excecao original.
        /// Isso e importante para rastreabilidade: permite saber qual erro
        /// tecnico causou o erro de negocio.
        /// </summary>
        [Fact]
        public void ConstrutorComInnerExceptionDevePreservarExcecaoOriginal()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var excecaoInterna = new InvalidOperationException("Erro no banco de dados.");
            var mensagem = "Erro ao cadastrar filme.";

            // ==========================================
            // ACT
            // ==========================================
            var excecao = new SenacFlixExcecao(mensagem, excecaoInterna);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(mensagem, excecao.Message);
            Assert.NotNull(excecao.InnerException);
            Assert.IsType<InvalidOperationException>(excecao.InnerException);
            Assert.Equal("Erro no banco de dados.", excecao.InnerException.Message);
        }

        /// <summary>
        /// Demonstra como usar Assert.Throws para verificar se uma excecao
        /// e lancada corretamente.
        /// 
        /// Assert.Throws<T>() recebe um delegate (Action ou Func) que
        /// contem o codigo que deve lancar a excecao. Se a excecao for
        /// lancada, o teste PASSA. Se NAO for lancada, o teste FALHA.
        /// 
        /// Este e um dos conceitos mais importantes em testes automatizados:
        /// verificar que o sistema reage corretamente a situacoes de erro.
        /// </summary>
        [Fact]
        public void DeveLancarSenacFlixExcecaoQuandoErroDeNegocioOcorrer()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Simulamos um metodo que lanca excecao ao encontrar um erro de negocio.
            var mensagemErro = "Categoria invalida para este filme.";

            // ==========================================
            // ACT + ASSERT
            // ==========================================
            // Assert.Throws verifica que a excecao do tipo SenacFlixExcecao
            // e realmente lancada pelo codigo dentro do delegate.
            // Nota: Act e Assert estao juntos aqui porque Assert.Throws
            // executa o codigo (Act) e verifica a excecao (Assert) simultaneamente.
            var excecaoCapturada = Assert.Throws<SenacFlixExcecao>((Action)(() =>
            {
                // Este e o codigo que deve lancar a excecao.
                throw new SenacFlixExcecao(mensagemErro);
            }));

            // Podemos tambem verificar a mensagem da excecao capturada.
            Assert.Equal(mensagemErro, excecaoCapturada.Message);
        }

        /// <summary>
        /// SenacFlixExcecao herda de Exception.
        /// Verificamos que a hierarquia de heranca esta correta.
        /// </summary>
        [Fact]
        public void SenacFlixExcecaoDeveHerdarDeException()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var excecao = new SenacFlixExcecao("Teste de heranca.");

            // ==========================================
            // ASSERT
            // ==========================================
            // Assert.IsAssignableFrom verifica que o objeto pode ser
            // tratado como o tipo base (Exception).
            Assert.IsAssignableFrom<Exception>(excecao);
        }
    }
}
