// ============================================================
// Nome:         SenacFlixExcecao.cs
// Objetivo:     Define a classe base de excecoes personalizadas do SenacFlix.
//               Permite que a camada de Application lance erros de negocio
//               semanticos, facilmente diferenciados de erros de infraestrutura.
// Camada:       Domain (Excecoes)
// Participa em: Lancada nos servicos de aplicacao quando regras de negocio sao violadas.
//               Capturada nos controllers/middlewares para retornar respostas HTTP adequadas.
//               Exemplos de uso: tentar favoritar um filme inexistente, cadastrar
//               titulo duplicado, usuario sem idade para acessar conteudo 18+.
// ============================================================

namespace SenacFlix.Domain.Excecoes
{
    /// <summary>
    /// Excecao base personalizada do dominio SenacFlix.
    /// Representa erros de regra de negocio que ocorrem na camada de Application.
    /// Herda de Exception para ser compativel com o mecanismo de excecoes do .NET.
    /// Use esta excecao (ou suas subclasses) para sinalizar violacoes de negocio.
    /// Nao use para erros de infraestrutura (banco de dados, rede, arquivos).
    /// </summary>
    public class SenacFlixExcecao : Exception // Herda de Exception: classe raiz de todas as excessões no .NET
    {
        //======================================================
        // Construtores
        //======================================================

        /// <summary>
        /// Construtor padrão sem argumentos
        /// Cria uma exceção com mensagem genérica
        /// Raramente usado diretamente; prefira os construtores com mensagem
        /// </summary>
        public SenacFlixExcecao() // Construtor sem parametros: chama o construtor base de Exception sem argumentos.
            : base("Ocorreu um erro na plataforma SenacFlix") // Mensagem padrão em português para o caso de uso sem mensagem específica

        { 
              // Corpo vazio: toda a lógica esta na chamada ao construtor base      
        }

        /// <summary>
        /// Construtor com mensagem de erro descritiva
        /// Use este construtor para informar ao usuario/log o que ocorreu errado
        /// </summary>
        /// <param name="mensagem">Descrição clara, detalhada e objetiva do erro de negócio ocorrido</param>
        public SenacFlixExcecao(string mensagem)
            : base(mensagem) 
        {
            // Corpo vazio: a mensagem é gerenciada pelo Exception base
        }

        /// <summary>
        /// Construtor com mensagem de exceção interna (inner exception).
        /// Use este construtor quando quiser encapsular uma excecao técnica (ex: DbException)
        /// </summary>
        /// <param name="mensagem">Descrição clara, detalhada e objetiva do erro de negócio ocorrido</param>
        public SenacFlixExcecao(string mensagem, Exception excecaoInterna) // Construtor  com inner exception para rastreabilidade
            : base(mensagem, excecaoInterna) // Repassa mensagem e exceção para o contrutor base de Exception
        {
            // Corpo vazio: a mensagem é gerenciada pelo Exception base
        }








    }
}
