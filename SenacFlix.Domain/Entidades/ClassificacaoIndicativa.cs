// ============================================================
// Nome:         ClassificacaoIndicativa.cs
// Objetivo:     Representa a classificacao indicativa (faixa etaria)
//               atribuida a cada filme do SenacFlix, seguindo o padrao
//               brasileiro do Ministerio da Justica.
//               Exemplos: Livre, 10 anos, 12 anos, 14 anos, 16 anos, 18 anos.
// Camada:       Domain (Entidades)
// Participa em: Relacionamento com Filme (1 classificacao para N filmes).
//               Usada para filtrar conteudo inadequado por faixa etaria.
// ============================================================

using System.ComponentModel.DataAnnotations; // Importa atributos de validacao de dados

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade que representa a classificacao indicativa de um filme.
    /// Cada filme possui exatamente uma classificacao que define
    /// a faixa etaria minima recomendada para assistir ao conteudo.
    /// </summary>
    public class ClassificacaoIndicativa // Classe de entidade do dominio sem heranca especial
    {
        // --------------------------------------------------------
        // Chave primaria
        // --------------------------------------------------------

        /// <summary>
        /// Identificador unico da classificacao indicativa no banco de dados.
        /// </summary>
        public int Id { get; set; } // Chave primaria reconhecida automaticamente pelo EF Core pela convencao de nome "Id"

        // --------------------------------------------------------
        // Dados da classificacao
        // --------------------------------------------------------

        /// <summary>
        /// Nome curto da classificacao para exibicao na interface.
        /// Exemplos: "Livre", "10+", "12+", "14+", "16+", "18+".
        /// Limitado a 20 caracteres pois e um codigo curto e padronizado.
        /// </summary>
        [MaxLength(20, ErrorMessage = "O nome da classificacao deve ter no maximo 20 caracteres.")] // Garante compacidade do campo no banco
        public required string Nome { get; set; } // required: campo obrigatorio na inicializacao do objeto

        /// <summary>
        /// Idade minima em anos que o usuario deve ter para assistir ao conteudo.
        /// Zero (0) indica que o conteudo e Livre para todas as idades.
        /// </summary>
        public int IdadeMinima { get; set; } // Inteiro positivo ou zero; zero = conteudo Livre

        /// <summary>
        /// Descricao detalhada sobre o tipo de conteudo desta classificacao.
        /// Pode conter os criterios que levaram a essa faixa etaria.
        /// Campo opcional pois o nome ja e suficientemente descritivo.
        /// </summary>
        public string? Descricao { get; set; } // Nullable: descricao complementar e opcional

        /// <summary>
        /// Cor em formato CSS associada a esta classificacao.
        /// Usada para exibir o selo colorido na interface do usuario.
        /// Exemplos: '#00AA00' (verde = Livre), '#FF0000' (vermelho = 18+).
        /// Limitado a 20 caracteres para comportar valores hexadecimais ou nomes CSS.
        /// </summary>
        [MaxLength(20, ErrorMessage = "A cor da classificacao deve ter no maximo 20 caracteres.")] // Cor CSS como '#RRGGBB' tem 7 chars; limite de 20 da margem para nomes
        public required string Cor { get; set; } // required: a cor e obrigatoria para renderizar o selo visual na UI
    }
}
