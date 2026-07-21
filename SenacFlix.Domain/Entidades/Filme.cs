// ============================================================
// Nome:         Filme.cs
// Objetivo:     Representa a entidade central da plataforma SenacFlix,
//               contendo todos os dados de um titulo audiovisual disponivel
//               no catalogo, desde informacoes basicas ate URLs de midia.
// Camada:       Domain (Entidades)
// Participa em: Relacionamento com Categoria, ClassificacaoIndicativa e Favorito.
//               E a entidade mais rica do sistema, manipulada pela maioria
//               dos casos de uso (listar, buscar, filtrar, favoritar).
// ============================================================

using System.ComponentModel.DataAnnotations; // Importa atributos de validacao como [MaxLength]

namespace SenacFlix.Domain.Entidades // Define o namespace da camada de dominio, pasta Entidades
{
    /// <summary>
    /// Entidade que representa um filme disponivel no catalogo do SenacFlix.
    /// Contem dados descritivos, links de midia, relacionamentos com categoria
    /// e classificacao indicativa, alem de controle de estado (ativo/inativo).
    /// </summary>
    public class Filme // Classe POCO principal do sistema SenacFlix
    {
        // --------------------------------------------------------
        // Chave primaria
        // --------------------------------------------------------

        /// <summary>
        /// Identificador unico do filme no banco de dados.
        /// Gerado automaticamente pelo EF Core (identity/autoincrement).
        /// </summary>
        public int Id { get; set; } // Chave primaria; o EF Core reconhece "Id" automaticamente por convencao

        // --------------------------------------------------------
        // Dados textuais principais
        // --------------------------------------------------------

        /// <summary>
        /// Titulo oficial do filme como sera exibido na plataforma.
        /// Obrigatorio com limite de 200 caracteres para acomodar titulos longos.
        /// </summary>
        [MaxLength(200, ErrorMessage = "O titulo do filme deve ter no maximo 200 caracteres.")] // Limita o campo no banco de dados
        public required string Titulo { get; set; } // required: titulo e obrigatorio; nenhum filme existe sem nome

        /// <summary>
        /// Sinopse ou descricao do enredo do filme.
        /// Obrigatoria para orientar o usuario sobre o conteudo antes de assistir.
        /// Sem limite de caracteres (TEXT no banco de dados).
        /// </summary>
        public required string Descricao { get; set; } // required: descricao e obrigatoria para o catalogo ser informativo

        /// <summary>
        /// Ano em que o filme foi lancado.
        /// Inteiro de quatro digitos. Exemplo: 2023.
        /// </summary>
        public int AnoLancamento { get; set; } // Inteiro puro; validacoes de intervalo sao feitas na camada de aplicacao

        /// <summary>
        /// Duracao total do filme em minutos.
        /// Exemplo: 120 = 2 horas. Exibido convertido na interface (ex: "2h 00min").
        /// </summary>
        public int Duracao { get; set; } // Duracao em minutos; a conversao para horas/minutos e feita na apresentacao

        /// <summary>
        /// Nome do diretor principal do filme.
        /// Campo opcional para catalogos incompletos; maximo de 150 caracteres.
        /// </summary>
        [MaxLength(150, ErrorMessage = "O nome do diretor deve ter no maximo 150 caracteres.")] // Limita o campo para nomes longos com seguranca
        public string? Diretor { get; set; } // Nullable: informacao pode nao estar disponivel para todos os filmes

        /// <summary>
        /// Lista dos atores e atrizes do elenco principal.
        /// Armazenada como string unica, com nomes separados por virgula.
        /// Exemplo: "Leonardo DiCaprio, Kate Winslet, Billy Zane".
        /// </summary>
        public string? Elenco { get; set; } // Nullable: lista de atores separados por virgula; pode ser nulo em catalogos incompletos

        // --------------------------------------------------------
        // URLs de midia e imagens
        // --------------------------------------------------------

        /// <summary>
        /// URL da imagem de capa do filme (formato vertical/poster).
        /// Exibida nos cards do catalogo e na pagina de detalhes do filme.
        /// </summary>
        public string? ImagemCapaUrl { get; set; } // Nullable: URL externa (CDN ou storage); pode ser nulo enquanto nao cadastrado

        /// <summary>
        /// URL da imagem de banner do filme (formato horizontal/landscape).
        /// Usada em destaques e sliders da pagina inicial.
        /// </summary>
        public string? ImagemBannerUrl { get; set; } // Nullable: banner e opcional; nem todos os filmes precisam de destaque

        /// <summary>
        /// URL do trailer oficial no YouTube.
        /// Exibido como preview antes do usuario decidir assistir ao filme.
        /// </summary>
        public string? TrailerYoutubeUrl { get; set; } // Nullable: link do YouTube para o trailer; pode ser nulo

        /// <summary>
        /// URL do video completo do filme no YouTube (ou outra plataforma).
        /// Este e o link principal que o usuario acessa para assistir ao conteudo.
        /// </summary>
        public string? VideoYoutubeUrl { get; set; } // Nullable: link do filme completo; pode ser nulo se ainda nao publicado

        // --------------------------------------------------------
        // Relacionamentos por chave estrangeira
        // --------------------------------------------------------

        /// <summary>
        /// Identificador da categoria a qual este filme pertence.
        /// Chave estrangeira que referencia a tabela Categorias.
        /// </summary>
        public int CategoriaId { get; set; } // FK para a tabela Categorias; obrigatorio (todo filme tem uma categoria)

        /// <summary>
        /// Objeto de navegacao para a categoria do filme.
        /// Preenchido pelo EF Core quando a consulta inclui o relacionamento (Include).
        /// </summary>
        public Categoria Categoria { get; set; } = null!; // null! suprime aviso de nullable; o EF Core garante o preenchimento em consultas com Include

        /// <summary>
        /// Identificador da classificacao indicativa atribuida ao filme.
        /// Chave estrangeira que referencia a tabela ClassificacoesIndicativas.
        /// </summary>
        public int ClassificacaoIndicativaId { get; set; } // FK para ClassificacaoIndicativa; obrigatorio por questoes legais

        /// <summary>
        /// Objeto de navegacao para a classificacao indicativa do filme.
        /// Preenchido pelo EF Core em consultas que incluem este relacionamento.
        /// </summary>
        public ClassificacaoIndicativa ClassificacaoIndicativa { get; set; } = null!; // null! pois o EF Core cuida do preenchimento em contexto de consulta

        // --------------------------------------------------------
        // Controle de estado (exclusao logica e auditoria simples)
        // --------------------------------------------------------

        /// <summary>
        /// Indica se o filme esta ativo e disponivel para visualizacao no catalogo.
        /// false = filme desativado, nao aparece para os usuarios (exclusao logica).
        /// </summary>
        public bool Ativo { get; set; } = true; // Padrao true: todo filme cadastrado fica disponivel imediatamente

        /// <summary>
        /// Indica se o filme devera ser promovido no Hero Banner da pagina inicial.
        /// </summary>
        public bool DestaqueHome { get; set; } = false;

        /// <summary>
        /// Data e hora em que o filme foi cadastrado no sistema.
        /// </summary>
        public DateTime DataCadastro { get; set; } // Registra o momento exato do cadastro

        /// <summary>
        /// Data e hora da ultima atualizacao nos dados do filme.
        /// Null se o filme nunca foi editado apos o cadastro inicial.
        /// </summary>
        public DateTime? DataAtualizacao { get; set; } // Nullable: preenchido somente quando o registro e editado

        /// <summary>
        /// Data e hora da exclusao logica do filme.
        /// Null enquanto o filme estiver ativo no sistema.
        /// </summary>
        public DateTime? DataExclusao { get; set; } // Nullable: preenchido somente ao desativar o filme

        // --------------------------------------------------------
        // Navegacao inversa
        // --------------------------------------------------------

        /// <summary>
        /// Colecao de registros de favoritos vinculados a este filme.
        /// Representa todos os usuarios que favoritaram este titulo.
        /// </summary>
        public ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>(); // Inicializado como lista vazia para evitar NullReferenceException
    }
}