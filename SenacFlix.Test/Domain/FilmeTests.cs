/*
 * ============================================================
 * Arquivo:   FilmeTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da entidade Filme do dominio SenacFlix.
 *
 * O que esta sendo testado:
 *   - Criacao de um Filme com dados validos.
 *   - Valores padrao das propriedades (Ativo, DestaqueHome).
 *   - Inicializacao da colecao de Favoritos.
 *   - Campos opcionais (nullable).
 *   - Atributos de validacao (MaxLength) via Reflection.
 *
 * Conceitos demonstrados:
 *   [Fact]       = teste com dados fixos e cenario unico.
 *   [Theory]     = teste executado com diferentes conjuntos de dados.
 *   [InlineData] = fornece os dados para cada execucao do Theory.
 *   Triple AAA   = Arrange (preparar), Act (executar), Assert (verificar).
 * ============================================================
 */

using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SenacFlix.Domain.Entidades;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da entidade Filme.
    /// Cada metodo testa um comportamento especifico da entidade.
    /// </summary>
    public class FilmeTests
    {
        // =====================================================
        // TESTES POSITIVOS — Cenarios Validos
        // =====================================================

        /// <summary>
        /// Verifica se um Filme pode ser criado com todos os dados validos.
        /// Este e o teste mais basico: garantir que a entidade funciona
        /// quando recebe dados corretos.
        /// 
        /// Utiliza [Fact] porque estamos testando um unico cenario fixo.
        /// </summary>
        [Fact]
        public void DeveCriarFilmeComDadosValidos()
        {
            // ==========================================
            // ARRANGE — Preparacao dos dados
            // ==========================================
            // Usamos o helper para criar um filme com dados validos.
            // Isso evita repetir a criacao do objeto em cada teste.

            // ==========================================
            // ACT — Execucao da acao
            // ==========================================
            // A "acao" aqui e a propria criacao do objeto.
            // Estamos verificando se ele pode ser instanciado corretamente.
            var filme = TestDataHelper.CriarFilmeValido();

            // ==========================================
            // ASSERT — Verificacao do resultado
            // ==========================================
            // Verificamos se cada propriedade foi preenchida corretamente.
            // Assert.NotNull verifica que o objeto nao e nulo.
            // Assert.Equal compara o valor esperado com o valor real.
            Assert.NotNull(filme);
            Assert.Equal("Interestelar", filme.Titulo);
            Assert.Equal("Uma equipe de exploradores viaja atraves de um buraco de minhoca no espaco.", filme.Descricao);
            Assert.Equal(2014, filme.AnoLancamento);
            Assert.Equal(169, filme.Duracao);
            Assert.Equal("Christopher Nolan", filme.Diretor);
            Assert.Equal(1, filme.CategoriaId);
            Assert.Equal(1, filme.ClassificacaoIndicativaId);
        }

        /// <summary>
        /// Verifica que todo filme novo e criado como Ativo por padrao.
        /// Esta e uma regra importante: filmes cadastrados devem estar
        /// disponiveis imediatamente no catalogo.
        /// </summary>
        [Fact]
        public void FilmeDeveSerAtivoPorPadrao()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            // Criamos um filme simples apenas com os campos obrigatorios.
            var filme = new Filme
            {
                Titulo = "Batman",
                Descricao = "O cavaleiro das trevas.",
                AnoLancamento = 2022,
                Duracao = 176,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            // ==========================================
            // ASSERT
            // ==========================================
            // Assert.True verifica que o valor e verdadeiro.
            Assert.True(filme.Ativo, "Todo filme deve ser criado como Ativo por padrao.");
        }

        /// <summary>
        /// Verifica que DestaqueHome e false por padrao.
        /// Apenas filmes selecionados devem aparecer no hero banner.
        /// </summary>
        [Fact]
        public void FilmeNaoDeveSerDestaquePorPadrao()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var filme = new Filme
            {
                Titulo = "Duna",
                Descricao = "Um jovem nobre aceita seu destino no deserto.",
                AnoLancamento = 2021,
                Duracao = 155,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            // ==========================================
            // ASSERT
            // ==========================================
            // Assert.False verifica que o valor e falso.
            Assert.False(filme.DestaqueHome, "DestaqueHome deve ser false por padrao.");
        }

        /// <summary>
        /// Verifica que a colecao de Favoritos e inicializada como lista vazia.
        /// Isso evita NullReferenceException ao acessar a colecao.
        /// </summary>
        [Fact]
        public void FilmeDevePossuirColecaoDeFavoritosInicializada()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var filme = new Filme
            {
                Titulo = "Oppenheimer",
                Descricao = "A historia do pai da bomba atomica.",
                AnoLancamento = 2023,
                Duracao = 180,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            // ==========================================
            // ASSERT
            // ==========================================
            // Assert.NotNull garante que a colecao foi inicializada.
            // Assert.Empty garante que a colecao comeca vazia.
            Assert.NotNull(filme.Favoritos);
            Assert.Empty(filme.Favoritos);
        }

        /// <summary>
        /// Verifica que campos opcionais (nullable) podem ser nulos.
        /// Diretor, Elenco, ImagemCapaUrl etc. sao opcionais no dominio.
        /// </summary>
        [Fact]
        public void FilmeDevePermitirCamposOpcionaisNulos()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var filme = new Filme
            {
                Titulo = "Filme Sem Detalhes",
                Descricao = "Um filme com poucos dados.",
                AnoLancamento = 2020,
                Duracao = 90,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
                // Campos opcionais nao preenchidos (null por padrao)
            };

            // ==========================================
            // ASSERT
            // ==========================================
            // Assert.Null verifica que o valor e nulo.
            Assert.Null(filme.Diretor);
            Assert.Null(filme.Elenco);
            Assert.Null(filme.ImagemCapaUrl);
            Assert.Null(filme.ImagemBannerUrl);
            Assert.Null(filme.TrailerYoutubeUrl);
            Assert.Null(filme.VideoYoutubeUrl);
            Assert.Null(filme.DataAtualizacao);
            Assert.Null(filme.DataExclusao);
        }

        // =====================================================
        // TESTES COM [THEORY] — Mesma regra, multiplas entradas
        // =====================================================

        /*
         * Por que usar [Theory] em vez de varios [Fact]?
         * 
         * Quando queremos testar a MESMA REGRA com DIFERENTES DADOS,
         * utilizamos [Theory] com [InlineData].
         * 
         * Isso evita criar um metodo separado para cada entrada.
         * Cada [InlineData] executa o teste com dados diferentes,
         * mas a logica do teste permanece a mesma.
         */

        /// <summary>
        /// Verifica que o atributo MaxLength(200) esta presente no Titulo.
        /// Usamos Reflection para confirmar que a restricao existe na entidade.
        /// 
        /// [Theory] e usado aqui para demonstrar como testar a mesma propriedade
        /// com diferentes valores de MaxLength esperados.
        /// </summary>
        [Theory]
        [InlineData("Titulo", 200)]
        [InlineData("Diretor", 150)]
        public void PropriedadeDeveTerMaxLengthCorreto(string nomePropriedade, int maxLengthEsperado)
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            // Obtemos informacoes da propriedade via Reflection.
            // Isso nos permite verificar os atributos de validacao declarados na entidade.
            var propriedade = typeof(Filme).GetProperty(nomePropriedade);

            // ==========================================
            // ACT
            // ==========================================
            // Buscamos o atributo MaxLength aplicado na propriedade.
            var atributo = propriedade?.GetCustomAttribute<MaxLengthAttribute>();

            // ==========================================
            // ASSERT
            // ==========================================
            // Verificamos que o atributo existe e que o valor de MaxLength esta correto.
            Assert.NotNull(atributo);
            Assert.Equal(maxLengthEsperado, atributo!.Length);
        }

        /// <summary>
        /// Verifica que um filme pode ter diferentes valores de duracao.
        /// Demonstra o uso de [Theory] com multiplos [InlineData].
        /// </summary>
        [Theory]
        [InlineData(90)]
        [InlineData(120)]
        [InlineData(180)]
        public void FilmeDeveAceitarDiferentesDuracoes(int duracao)
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var filme = new Filme
            {
                Titulo = "Filme Teste",
                Descricao = "Descricao teste.",
                AnoLancamento = 2024,
                Duracao = duracao,
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Equal(duracao, filme.Duracao);
        }
    }
}
