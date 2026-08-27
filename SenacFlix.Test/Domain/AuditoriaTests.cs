/*
 * ============================================================
 * Arquivo:   AuditoriaTests.cs
 * Camada:    SenacFlix.Test / Domain
 * Finalidade:
 *   Testes automatizados da entidade Auditoria do dominio SenacFlix.
 *
 * O que esta sendo testado:
 *   - Criacao de um registro de Auditoria com dados validos.
 *   - Campos opcionais (UsuarioId, NomeUsuario, Detalhes).
 *
 * Conceitos demonstrados:
 *   [Fact]     = teste com cenario fixo.
 *   Triple AAA = Arrange, Act, Assert.
 * ============================================================
 */

using SenacFlix.Domain.Entidades;
using SenacFlix.Test.Helpers;

namespace SenacFlix.Test.Domain
{
    /// <summary>
    /// Classe de testes da entidade Auditoria.
    /// Registros de auditoria gravam acoes realizadas no sistema.
    /// </summary>
    public class AuditoriaTests
    {
        /// <summary>
        /// Verifica se uma Auditoria pode ser criada com dados validos.
        /// </summary>
        [Fact]
        public void DeveCriarAuditoriaComDadosValidos()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            var auditoria = TestDataHelper.CriarAuditoriaValida();

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.NotNull(auditoria);
            Assert.Equal("Criacao", auditoria.Acao);
            Assert.Equal("Filmes", auditoria.TabelaAfetada);
            Assert.Equal("Joao Silva", auditoria.NomeUsuario);
        }

        /// <summary>
        /// Acoes do sistema (automatizadas) podem nao ter usuario associado.
        /// UsuarioId e NomeUsuario sao opcionais (nullable).
        /// </summary>
        [Fact]
        public void AuditoriaDevePermitirUsuarioNulo()
        {
            // ==========================================
            // ARRANGE + ACT
            // ==========================================
            // Simulamos uma acao do sistema sem usuario logado.
            var auditoria = new Auditoria
            {
                Acao = "Criacao",
                TabelaAfetada = "Categorias",
                DataHora = DateTime.UtcNow
                // UsuarioId e NomeUsuario nao preenchidos (null)
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.Null(auditoria.UsuarioId);
            Assert.Null(auditoria.NomeUsuario);
            Assert.Null(auditoria.Detalhes);
        }

        /// <summary>
        /// Verifica que a DataHora e registrada corretamente.
        /// </summary>
        [Fact]
        public void AuditoriaDeveRegistrarDataHora()
        {
            // ==========================================
            // ARRANGE
            // ==========================================
            var antesDoTeste = DateTime.UtcNow;

            // ==========================================
            // ACT
            // ==========================================
            var auditoria = new Auditoria
            {
                Acao = "Login",
                TabelaAfetada = "AspNetUsers",
                DataHora = DateTime.UtcNow
            };

            // ==========================================
            // ASSERT
            // ==========================================
            Assert.True(auditoria.DataHora >= antesDoTeste,
                "A DataHora da auditoria deve ser registrada no momento da criacao.");
        }
    }
}
