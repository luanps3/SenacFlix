/*
 * ============================================================
 * Arquivo:   FilmeDtoTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da propriedade calculada DuracaoFormatada
 *   presente no FilmeDto.
 *
 * O que esta sendo testado:
 *   - Formatacao correta da duracao em minutos para texto legivel.
 *   - Cenarios: zero/negativo, somente minutos, somente horas, horas e minutos.
 *
 * Por que testar uma propriedade calculada?
 *   DuracaoFormatada contem logica de negocio (conversao de minutos
 *   para formato "Xh Ymin"). Propriedades com logica devem ser testadas.
 *   Propriedades simples (get/set) NAO devem ser testadas pois sao
 *   funcionalidades do framework C#.
 *
 * Conceitos demonstrados:
 *   [Theory]     = teste executado com diferentes valores de entrada.
 *   [InlineData] = fornece cada combinacao de duracao e resultado esperado.
 *   Triple AAA   = Arrange, Act, Assert.
 *
 * EXEMPLO DIDATICO PRINCIPAL:
 *   Este arquivo demonstra a diferenca entre [Fact] e [Theory].
 *   Veja o comentario detalhado abaixo.
 * ============================================================
 */

using SenacFlix.Application.DTOs;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da propriedade calculada DuracaoFormatada do FilmeDto.
    /// </summary>
    public class FilmeDtoTests
    {
        /*
         * ============================================================
         * EXEMPLO DIDATICO: [Fact] vs [Theory]
         * ============================================================
         *
         * Se usassemos [Fact], precisariamos criar um metodo SEPARADO
         * para cada cenario de duracao:
         *
         *     [Fact]
         *     public void DuracaoZeroDeveRetornarDesconhecida() { ... }
         *
         *     [Fact]
         *     public void Duracao45DeveRetornar45min() { ... }
         *
         *     [Fact]
         *     public void Duracao60DeveRetornar1h() { ... }
         *
         *     [Fact]
         *     public void Duracao90DeveRetornar1h30min() { ... }
         *
         *     [Fact]
         *     public void Duracao169DeveRetornar2h49min() { ... }
         *
         * Com [Theory] + [InlineData], escrevemos a logica UMA UNICA VEZ
         * e o xUnit executa automaticamente o teste para cada entrada.
         * Isso torna o codigo mais limpo, mais facil de manter e
         * permite adicionar novos cenarios simplesmente adicionando
         * mais linhas de [InlineData].
         * ============================================================
         */

        /// <summary>
        /// Testa a conversao de duracao em minutos para texto formatado.
        /// 
        /// Exemplos:
        ///   0   -> "Desconhecida" (duracao invalida)
        ///   -1  -> "Desconhecida" (duracao negativa)
        ///   45  -> "45min" (somente minutos)
        ///   60  -> "1h" (somente horas)
        ///   90  -> "1h 30min" (horas e minutos)
        ///   169 -> "2h 49min" (Interestelar: 2 horas e 49 minutos)
        /// </summary>
        [Theory]
        [InlineData(0, "Desconhecida")]
        [InlineData(-1, "Desconhecida")]
        [InlineData(45, "45min")]
        [InlineData(60, "1h")]
        [InlineData(90, "1h 30min")]
        [InlineData(120, "2h")]
        [InlineData(169, "2h 49min")]
        public void DuracaoFormatadaDeveConverterMinutosParaTexto(int duracaoMinutos, string resultadoEsperado)
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Criamos um FilmeDto e definimos a duracao em minutos.
            var filmeDto = new FilmeDto
            {
                Duracao = duracaoMinutos
            };

            // ==========================================
            // ACT
            // ==========================================
            // Acessamos a propriedade calculada DuracaoFormatada.
            // Esta propriedade contem a logica de conversao.
            var resultado = filmeDto.DuracaoFormatada;

            // ==========================================
            // ASSERT
            // ==========================================
            // Verificamos se o resultado da conversao e o esperado.
            Assert.Equal(resultadoEsperado, resultado);
        }
    }
}
