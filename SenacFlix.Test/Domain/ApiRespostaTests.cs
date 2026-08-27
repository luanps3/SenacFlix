/*
 * ============================================================
 * Arquivo:   ApiRespostaTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da classe ApiResposta<T>.
 *
 * O que esta sendo testado:
 *   - Metodo estatico Ok() que cria uma resposta de sucesso.
 *   - Metodo estatico Falha() que cria uma resposta de erro.
 *   - Metodo estatico FalhaValidacao() que cria uma resposta com lista de erros.
 *
 * Por que testar o ApiResposta?
 *   Todos os services do SenacFlix retornam ApiResposta<T>.
 *   E fundamental garantir que este wrapper funciona corretamente,
 *   pois todas as respostas da API dependem dele.
 *
 * Conceitos demonstrados:
 *   [Fact]     = teste com cenario fixo.
 *   Triple AAA = Arrange, Act, Assert.
 * ============================================================
 */

using SenacFlix.Application.DTOs;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes do wrapper ApiResposta.
    /// Testa os metodos estaticos Ok, Falha e FalhaValidacao.
    /// </summary>
    public class ApiRespostaTests
    {
        /// <summary>
        /// O metodo Ok() deve retornar uma resposta com Sucesso = true
        /// e os dados informados.
        /// </summary>
        [Fact]
        public void OkDeveRetornarSucessoComDados()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var dadosTeste = "Dados de exemplo";

            // ==========================================
            // ACT
            // ==========================================
            var resposta = ApiResposta<string>.Ok(dadosTeste, "Tudo certo.");

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resposta.Sucesso, "Ok() deve retornar Sucesso = true.");
            Assert.Equal("Tudo certo.", resposta.Mensagem);
            Assert.Equal("Dados de exemplo", resposta.Dados);
        }

        /// <summary>
        /// O metodo Ok() deve usar mensagem padrao quando nenhuma mensagem for informada.
        /// </summary>
        [Fact]
        public void OkDeveUsarMensagemPadraoQuandoNaoInformada()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var resposta = ApiResposta<int>.Ok(42);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(resposta.Sucesso);
            Assert.Equal("Operacao realizada com sucesso.", resposta.Mensagem);
            Assert.Equal(42, resposta.Dados);
        }

        /// <summary>
        /// O metodo Falha() deve retornar uma resposta com Sucesso = false
        /// e a mensagem de erro informada.
        /// </summary>
        [Fact]
        public void FalhaDeveRetornarErroComMensagem()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var mensagemErro = "Filme nao encontrado.";

            // ==========================================
            // ACT
            // ==========================================
            var resposta = ApiResposta<string>.Falha(mensagemErro);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resposta.Sucesso, "Falha() deve retornar Sucesso = false.");
            Assert.Equal("Filme nao encontrado.", resposta.Mensagem);
            // Quando a operacao falha, Dados deve ser o valor padrao do tipo (null para string).
            Assert.Null(resposta.Dados);
        }

        /// <summary>
        /// O metodo FalhaValidacao() deve retornar uma lista de erros.
        /// Usado quando ha multiplos erros de validacao.
        /// </summary>
        [Fact]
        public void FalhaValidacaoDeveRetornarListaDeErros()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var erros = new List<string>
            {
                "O Titulo e obrigatorio.",
                "A Descricao e obrigatoria.",
                "A Categoria e obrigatoria."
            };

            // ==========================================
            // ACT
            // ==========================================
            var resposta = ApiResposta<string>.FalhaValidacao(erros);

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.False(resposta.Sucesso);
            Assert.Equal("Erro de validacao.", resposta.Mensagem);
            Assert.NotNull(resposta.Erros);
            Assert.Equal(3, resposta.Erros!.Count);
            Assert.Contains("O Titulo e obrigatorio.", resposta.Erros);
        }
    }
}
