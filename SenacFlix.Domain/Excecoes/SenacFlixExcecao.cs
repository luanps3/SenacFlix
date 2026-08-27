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

namespace SenacFlix.Domain.Excecoes // Define o namespace da camada de dominio, pasta Excecoes
{
    /// <summary>
    /// Excecao base personalizada do dominio SenacFlix.
    /// Representa erros de regra de negocio que ocorrem na camada de Application.
    /// Herda de Exception para ser compativel com o mecanismo de excecoes do .NET.
    /// Use esta excecao (ou suas subclasses) para sinalizar violacoes de negocio.
    /// Nao use para erros de infraestrutura (banco de dados, rede, arquivos).
    /// </summary>
    public class SenacFlixExcecao : Exception // Herda de Exception: classe raiz de todas as excecoes no .NET
    {
        // --------------------------------------------------------
        // Construtores
        // --------------------------------------------------------

        /// <summary>
        /// Construtor padrao sem argumentos.
        /// Cria uma excecao com mensagem generica.
        /// Raramente usado diretamente; prefira os construtores com mensagem.
        /// </summary>
        public SenacFlixExcecao() // Construtor sem parametros: chama o construtor base de Exception sem argumentos
            : base("Ocorreu um erro na plataforma SenacFlix.") // Mensagem padrao em portugues para o caso de uso sem mensagem especifica
        {
            // Corpo vazio: toda a logica esta na chamada ao construtor base
        }

        /// <summary>
        /// Construtor com mensagem de erro descritiva.
        /// Use este construtor para informar ao usuario/log o que ocorreu de errado.
        /// </summary>
        /// <param name="mensagem">Descricao clara e objetiva do erro de negocio ocorrido.</param>
        public SenacFlixExcecao(string mensagem) // Construtor com mensagem: o mais utilizado no dia a dia
            : base(mensagem) // Repassa a mensagem para o construtor base de Exception
        {
            // Corpo vazio: a mensagem e gerenciada pelo Exception base
        }

        /// <summary>
        /// Construtor com mensagem e excecao interna (inner exception).
        /// Use este construtor quando quiser encapsular uma excecao tecnica (ex: DbException)
        /// dentro de uma excecao de negocio, preservando o rastreamento de pilha original.
        /// </summary>
        /// <param name="mensagem">Descricao do erro de negocio.</param>
        /// <param name="excecaoInterna">A excecao tecnica original que causou este erro.</param>
        public SenacFlixExcecao(string mensagem, Exception excecaoInterna) // Construtor com inner exception para rastreabilidade
            : base(mensagem, excecaoInterna) // Repassa mensagem e excecao interna para o construtor base de Exception
        {
            // Corpo vazio: tanto a mensagem quanto a excecao interna sao gerenciadas pelo Exception base
        }
    }
}
