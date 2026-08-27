/*
 * ============================================================
 * Arquivo:   TestDataHelper.cs
 * Camada:    SenacFlix.Test / Helpers
 * Finalidade:
 *   Centralizar a criacao de objetos de teste validos.
 *   Cada metodo deste helper retorna uma instancia com dados
 *   validos, pronta para ser usada na etapa ARRANGE dos testes.
 *
 * Por que usar um Helper?
 *   Evita duplicar a criacao de objetos em cada teste.
 *   Se a entidade mudar, basta ajustar aqui em um unico lugar.
 *
 * Conceitos demonstrados:
 *   - Factory Method para testes
 *   - Dados de teste realistas e validos
 * ============================================================
 */

using SenacFlix.Application.DTOs;
using SenacFlix.Domain.Entidades;

namespace SenacFlix.Test.Helpers
{
    /// <summary>
    /// Classe auxiliar que fornece metodos para criar objetos
    /// de teste com dados validos. Usada na etapa ARRANGE dos testes.
    /// </summary>
    public static class TestDataHelper
    {
        // --------------------------------------------------------
        // ENTIDADES DE DOMINIO
        // --------------------------------------------------------

        /// <summary>
        /// Cria um objeto Filme com todos os dados validos preenchidos.
        /// </summary>
        public static Filme CriarFilmeValido()
        {
            return new Filme
            {
                Id = 1,
                Titulo = "Interestelar",
                Descricao = "Uma equipe de exploradores viaja atraves de um buraco de minhoca no espaco.",
                AnoLancamento = 2014,
                Duracao = 169,
                Diretor = "Christopher Nolan",
                Elenco = "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
                ImagemCapaUrl = "https://exemplo.com/interestelar-capa.jpg",
                ImagemBannerUrl = "https://exemplo.com/interestelar-banner.jpg",
                TrailerYoutubeUrl = "https://youtube.com/watch?v=abc123",
                VideoYoutubeUrl = "https://youtube.com/watch?v=xyz789",
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1,
                Ativo = true,
                DestaqueHome = false,
                DataCadastro = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Cria um objeto Categoria com dados validos.
        /// </summary>
        public static Categoria CriarCategoriaValida()
        {
            return new Categoria
            {
                Id = 1,
                Nome = "Ficcao Cientifica",
                Descricao = "Filmes de ficcao cientifica e espaco.",
                Ativo = true,
                DataCadastro = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Cria um objeto ClassificacaoIndicativa com dados validos.
        /// </summary>
        public static ClassificacaoIndicativa CriarClassificacaoValida()
        {
            return new ClassificacaoIndicativa
            {
                Id = 1,
                Nome = "12+",
                IdadeMinima = 12,
                Descricao = "Nao recomendado para menores de 12 anos.",
                Cor = "#F5C518"
            };
        }

        /// <summary>
        /// Cria um objeto Favorito com dados validos.
        /// </summary>
        public static Favorito CriarFavoritoValido()
        {
            return new Favorito
            {
                Id = 1,
                UsuarioId = "usuario-guid-123",
                FilmeId = 1,
                DataFavorito = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Cria um objeto Auditoria com dados validos.
        /// </summary>
        public static Auditoria CriarAuditoriaValida()
        {
            return new Auditoria
            {
                Id = 1,
                UsuarioId = "usuario-guid-123",
                NomeUsuario = "Joao Silva",
                Acao = "Criacao",
                TabelaAfetada = "Filmes",
                Detalhes = "Filme 'Interestelar' cadastrado.",
                DataHora = DateTime.UtcNow
            };
        }

        // --------------------------------------------------------
        // DTOs (OBJETOS DE TRANSFERENCIA DE DADOS)
        // --------------------------------------------------------

        /// <summary>
        /// Cria um CriarFilmeDto com dados validos para cadastro.
        /// </summary>
        public static CriarFilmeDto CriarFilmeDtoValido()
        {
            return new CriarFilmeDto
            {
                Titulo = "Duna: Parte Dois",
                Descricao = "Paul Atreides se une aos Fremen para vingar sua familia.",
                AnoLancamento = 2024,
                Duracao = 166,
                Diretor = "Denis Villeneuve",
                Elenco = "Timothee Chalamet, Zendaya, Austin Butler",
                ImagemCapaUrl = "https://exemplo.com/duna2-capa.jpg",
                CategoriaId = 1,
                ClassificacaoIndicativaId = 1
            };
        }

        /// <summary>
        /// Cria um CriarCategoriaDto com dados validos para cadastro.
        /// </summary>
        public static CriarCategoriaDto CriarCategoriaDtoValido()
        {
            return new CriarCategoriaDto
            {
                Nome = "Acao",
                Descricao = "Filmes de acao e aventura."
            };
        }

        /// <summary>
        /// Cria um AdicionarFavoritoDto com dados validos.
        /// </summary>
        public static AdicionarFavoritoDto CriarAdicionarFavoritoDtoValido()
        {
            return new AdicionarFavoritoDto
            {
                FilmeId = 1
            };
        }
    }
}
