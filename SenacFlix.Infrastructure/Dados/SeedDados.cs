// Nome do arquivo: SeedDados.cs
// Objetivo: Popular o banco com um catalogo de 50 filmes populares com dados completos e consistentes.
//           Cada filme possui seu proprio poster, banner e trailer oficiais.
//           Garante criacao de Roles e Usuarios padrao via Identity.
// Camada: Infrastructure

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SenacFlix.Domain.Entidades;
using SenacFlix.Infrastructure.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenacFlix.Infrastructure.Dados
{
    public static class SeedDados
    {
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SenacFlixContexto>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Aplica migrations pendentes automaticamente
            context.Database.Migrate();

            // ================================================================
            // SEED DE IDENTITY: Roles e Usuarios padrao (idempotente)
            // Deve ser executado ANTES do seed de dados para que o usuario
            // Cliente possa ter favoritos associados.
            // ================================================================
            await SeedIdentityAsync(userManager, roleManager);

            // Remove favoritos antes dos filmes para respeitar FK
            if (context.Favoritos.Any())
            {
                context.Favoritos.RemoveRange(context.Favoritos);
                context.SaveChanges();
            }

            // Remove filmes para recriar catalogo limpo
            if (context.Filmes.Any())
            {
                context.Filmes.RemoveRange(context.Filmes);
                context.SaveChanges();
            }

            // ================================================================
            // SEED DE CATEGORIAS (idempotente)
            // ================================================================
            {
                var todasCategorias = new List<Categoria>
                {
                    new Categoria { Nome = "Ação" },
                    new Categoria { Nome = "Ficção Científica" },
                    new Categoria { Nome = "Drama" },
                    new Categoria { Nome = "Aventura" },
                    new Categoria { Nome = "Comédia" },
                    new Categoria { Nome = "Terror" },
                    new Categoria { Nome = "Suspense" },
                    new Categoria { Nome = "Animação" }
                };
                var nomesCategoriasExistentes = context.Categorias.Select(c => c.Nome).ToHashSet();
                var novasCategorias = todasCategorias.Where(c => !nomesCategoriasExistentes.Contains(c.Nome)).ToList();
                if (novasCategorias.Any())
                {
                    context.Categorias.AddRange(novasCategorias);
                    context.SaveChanges();
                }
            }

            // ================================================================
            // SEED DE CLASSIFICACOES INDICATIVAS (idempotente)
            // ================================================================
            {
                var todasClassificacoes = new List<ClassificacaoIndicativa>
                {
                    new ClassificacaoIndicativa { Nome = "Livre",  Descricao = "Livre para todos os públicos",                      IdadeMinima = 0,  Cor = "#00B150" },
                    new ClassificacaoIndicativa { Nome = "10",     Descricao = "Não recomendado para menores de 10 anos",            IdadeMinima = 10, Cor = "#00AEEF" },
                    new ClassificacaoIndicativa { Nome = "12",     Descricao = "Não recomendado para menores de 12 anos",            IdadeMinima = 12, Cor = "#F8C300" },
                    new ClassificacaoIndicativa { Nome = "14",     Descricao = "Não recomendado para menores de 14 anos",            IdadeMinima = 14, Cor = "#F37021" },
                    new ClassificacaoIndicativa { Nome = "16",     Descricao = "Não recomendado para menores de 16 anos",            IdadeMinima = 16, Cor = "#ED1C24" },
                    new ClassificacaoIndicativa { Nome = "18",     Descricao = "Não recomendado para menores de 18 anos",            IdadeMinima = 18, Cor = "#000000" }
                };
                var nomesClassifExistentes = context.ClassificacoesIndicativas.Select(c => c.Nome).ToHashSet();
                var novasClassificacoes = todasClassificacoes.Where(c => !nomesClassifExistentes.Contains(c.Nome)).ToList();
                if (novasClassificacoes.Any())
                {
                    context.ClassificacoesIndicativas.AddRange(novasClassificacoes);
                    context.SaveChanges();
                }
            }

            // ================================================================
            // SEED DE FILMES — 50 filmes populares com dados completos
            // Cada filme possui:
            //   - Poster real (TMDB CDN publico, sem necessidade de API key)
            //   - Banner real (TMDB CDN publico, sem necessidade de API key)
            //   - Trailer oficial unico do YouTube (embed)
            //   - VideoYoutubeUrl = TrailerYoutubeUrl (conforme politica da plataforma)
            //   - Classificacao indicativa correta
            //   - Categoria correta
            // ================================================================
            if (!context.Filmes.Any())
            {
                // Recupera IDs das categorias e classificacoes
                var catAcao     = context.Categorias.First(c => c.Nome == "Ação").Id;
                var catFiccao   = context.Categorias.First(c => c.Nome == "Ficção Científica").Id;
                var catDrama    = context.Categorias.First(c => c.Nome == "Drama").Id;
                var catAventura = context.Categorias.First(c => c.Nome == "Aventura").Id;
                var catComedia  = context.Categorias.First(c => c.Nome == "Comédia").Id;
                var catTerror   = context.Categorias.First(c => c.Nome == "Terror").Id;
                var catSuspense = context.Categorias.First(c => c.Nome == "Suspense").Id;
                var catAnimacao = context.Categorias.First(c => c.Nome == "Animação").Id;

                var classLivre = context.ClassificacoesIndicativas.First(c => c.Nome == "Livre").Id;
                var class10    = context.ClassificacoesIndicativas.First(c => c.Nome == "10").Id;
                var class12    = context.ClassificacoesIndicativas.First(c => c.Nome == "12").Id;
                var class14    = context.ClassificacoesIndicativas.First(c => c.Nome == "14").Id;
                var class16    = context.ClassificacoesIndicativas.First(c => c.Nome == "16").Id;
                var class18    = context.ClassificacoesIndicativas.First(c => c.Nome == "18").Id;

                var filmes = new List<Filme>
                {
                    // ── 01. Interestelar ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Interestelar",
                        AnoLancamento          = 2014,
                        Duracao                = 169,
                        Descricao              = "Um grupo de exploradores faz uso de um buraco de minhoca recém-descoberto para superar as limitações das viagens espaciais humanas e conquistar as vastidões do universo.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Matthew McConaughey, Anne Hathaway, Jessica Chastain, Michael Caine",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/zSWdZVtXT7E",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/zSWdZVtXT7E",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 02. Oppenheimer ──────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Oppenheimer",
                        AnoLancamento          = 2023,
                        Duracao                = 180,
                        Descricao              = "A história do físico J. Robert Oppenheimer e sua função no Projeto Manhattan, que levou ao desenvolvimento das primeiras armas nucleares durante a Segunda Guerra Mundial.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Cillian Murphy, Emily Blunt, Robert Downey Jr., Matt Damon",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/8Gxv8gSFCU0XGDykEGv7zR1n2ua.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/rLb2cwF3Pazuxaj0sRXQ037tGI1.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/uYPbbksJxIg",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/uYPbbksJxIg",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 03. Duna ─────────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Duna",
                        AnoLancamento          = 2021,
                        Duracao                = 155,
                        Descricao              = "O jovem Paul Atreides, filho de uma família nobre, chega ao planeta mais perigoso do universo para garantir o futuro de sua família e de seu povo. Mas antes disso, terá que superar seus próprios medos.",
                        Diretor                = "Denis Villeneuve",
                        Elenco                 = "Timothée Chalamet, Rebecca Ferguson, Oscar Isaac, Zendaya",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/d5NXSklXo0qyIYkgV48Ziti0dkx.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/iopYFB1b6Bh7FWZh3onQhph1sih.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/8g18jFHCLXk",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/8g18jFHCLXk",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 04. Duna: Parte Dois ─────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Duna: Parte Dois",
                        AnoLancamento          = 2024,
                        Duracao                = 166,
                        Descricao              = "Paul Atreides se une a Chani e aos Fremen enquanto busca vingança contra os conspiradores que destruíram sua família. Diante de uma escolha entre o amor de sua vida e o destino do universo, ele se esforça para evitar um futuro aterrorizante.",
                        Diretor                = "Denis Villeneuve",
                        Elenco                 = "Timothée Chalamet, Zendaya, Rebecca Ferguson, Austin Butler",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/1pdfLvkbY9ohJlCjQH2CZjjYVvJ.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/xOMo8BRK7PfcJv9JCnx7s5hj0PX.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/Way9Dexny3w",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/Way9Dexny3w",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 05. Batman Begins ────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Batman Begins",
                        AnoLancamento          = 2005,
                        Duracao                = 140,
                        Descricao              = "Após treinar com seu mentor, Bruce Wayne retorna para Gotham City e começa a combater os criminosos que destruíram sua vida como o vigilante mascarado Batman.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Christian Bale, Michael Caine, Liam Neeson, Katie Holmes",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/8RW2runSEc34IwKN2D1aPcJd2UL.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/uBtwS8hJ64LS8BBaiKHzcxsdrLs.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/neY2xVmOfUM",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/neY2xVmOfUM",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 06. O Cavaleiro das Trevas ───────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "O Cavaleiro das Trevas",
                        AnoLancamento          = 2008,
                        Duracao                = 152,
                        Descricao              = "Quando o Coringa mergulha Gotham City no caos, Batman deve aceitar uma das maiores provas psicológicas de sua capacidade de combater a injustiça.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Christian Bale, Heath Ledger, Aaron Eckhart, Maggie Gyllenhaal",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/hqkIcbrOHL86UncnHIsHVcVmzue.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/EXeTwQWrcwY",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/EXeTwQWrcwY",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 07. Coringa ──────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Coringa",
                        AnoLancamento          = 2019,
                        Duracao                = 122,
                        Descricao              = "Em Gotham City, o comediante fracassado Arthur Fleck é desrespeitado e ignorado pela sociedade. Ele então embarca em uma jornada descendente para a loucura e se torna o Coringa.",
                        Diretor                = "Todd Phillips",
                        Elenco                 = "Joaquin Phoenix, Robert De Niro, Zazie Beetz, Frances Conroy",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/udDclJoHjfjb8Ekgsd4FDteOkCU.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/f5F4cRhQdUbyVbB5lTNCwFMTDoa.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/zAGVQLHvwOY",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/zAGVQLHvwOY",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 08. Avatar ───────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Avatar",
                        AnoLancamento          = 2009,
                        Duracao                = 162,
                        Descricao              = "Um ex-fuzileiro naval paraplégico é enviado para a lua Pandora em missão diplomática. Dividido entre suas ordens e o amor por seu novo lar, ele deve liderar uma batalha épica pelo destino de um mundo.",
                        Diretor                = "James Cameron",
                        Elenco                 = "Sam Worthington, Zoe Saldana, Sigourney Weaver, Michelle Rodriguez",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/jRXYjXNq0Cs2TcJjLkki24MLp7u.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/s16H6tpK2utvwpapmrhDo0U3yoD.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/5PSNL1qE6VY",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/5PSNL1qE6VY",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 09. Avatar: O Caminho da Água ────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Avatar: O Caminho da Água",
                        AnoLancamento          = 2022,
                        Duracao                = 192,
                        Descricao              = "Jake Sully vive com sua nova família em Pandora. Quando uma antiga ameaça retorna para terminar o que havia começado, Jake deve trabalhar com Neytiri e o exército dos Na'vi para proteger seu planeta.",
                        Diretor                = "James Cameron",
                        Elenco                 = "Sam Worthington, Zoe Saldana, Sigourney Weaver, Kate Winslet",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/4SzT8EZr3WyWrBLimJMH7K7VFZZ.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/d9MyW72ELq0",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/d9MyW72ELq0",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 10. Vingadores: Ultimato ─────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Vingadores: Ultimato",
                        AnoLancamento          = 2019,
                        Duracao                = 181,
                        Descricao              = "Após os eventos devastadores de Vingadores: Guerra Infinita, o universo está em ruínas. Com a ajuda de aliados restantes, os Vingadores se reúnem mais uma vez para reverter as ações de Thanos e restaurar o equilíbrio no universo.",
                        Diretor                = "Anthony Russo, Joe Russo",
                        Elenco                 = "Robert Downey Jr., Chris Evans, Mark Ruffalo, Chris Hemsworth",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/7RyHsO4yDXtBv1zUU3mTpHeQ0d5.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/TcMBFSGVi1c",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/TcMBFSGVi1c",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 11. Homem de Ferro ───────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Homem de Ferro",
                        AnoLancamento          = 2008,
                        Duracao                = 126,
                        Descricao              = "Tony Stark, um gênio milionário e fabricante de armas, é capturado por terroristas e forçado a construir uma arma de destruição. Em vez disso, constrói uma armadura de alta tecnologia e a usa para escapar e tornar-se o Homem de Ferro.",
                        Diretor                = "Jon Favreau",
                        Elenco                 = "Robert Downey Jr., Gwyneth Paltrow, Jeff Bridges, Terrence Howard",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/78lPtwv72eTNqFW9COBF8l6T0qg.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/cyecB7godJ6kNHGONFjUyVN9OX5.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/8ugaeA-nMTc",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/8ugaeA-nMTc",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 12. Doutor Estranho ──────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Doutor Estranho",
                        AnoLancamento          = 2016,
                        Duracao                = 115,
                        Descricao              = "O Dr. Stephen Strange, um neurocirurgião de elite, descobre o mundo oculto da magia e dimensões alternativas depois de um acidente de carro que arruinou sua carreira.",
                        Diretor                = "Scott Derrickson",
                        Elenco                 = "Benedict Cumberbatch, Chiwetel Ejiofor, Rachel McAdams, Tilda Swinton",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/uGBVl7yu7QoD5Vc3N47Zft9BQFN.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/hbhFnRzzg6ZDmm8YAmxBnmtM9S5.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/HSzx-zryEgM",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/HSzx-zryEgM",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 13. Top Gun: Maverick ────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Top Gun: Maverick",
                        AnoLancamento          = 2022,
                        Duracao                = 130,
                        Descricao              = "Depois de mais de 30 anos de serviço como um dos melhores aviadores da Marinha, Pete 'Maverick' Mitchell está de volta como treinador de uma nova geração de pilotos.",
                        Diretor                = "Joseph Kosinski",
                        Elenco                 = "Tom Cruise, Miles Teller, Jennifer Connelly, Jon Hamm",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/62HCnUTHjWTObPnSPIOnczjmf5P.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/14QbnygCuTO0vl7CAFmPf1fgZfV.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/qSqVVswa420",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/qSqVVswa420",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 14. John Wick ─────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "John Wick",
                        AnoLancamento          = 2014,
                        Duracao                = 101,
                        Descricao              = "Um ex-assassino sai da aposentadoria para procurar os gangsters que roubaram seu carro e mataram seu cachorro, um presente de seu amor falecido.",
                        Diretor                = "Chad Stahelski",
                        Elenco                 = "Keanu Reeves, Michael Nyqvist, Alfie Allen, Adrianne Palicki",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/fZPSd91yGE9fCcCe6OoQr6E3Bev.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/mMtUybQ6hL24FXo0F3Z4j2KG7kQ.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/2AUmvWm5ZDQ",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/2AUmvWm5ZDQ",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 15. Parasita ─────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Parasita",
                        AnoLancamento          = 2019,
                        Duracao                = 132,
                        Descricao              = "A família Ki-taek vive em situação de pobreza extrema em uma casa semi-subterrânea. Quando o filho mais velho consegue emprego de tutor na família rica Park, os Ki-taek passam a infiltrar-se cada vez mais na vida dos empregadores.",
                        Diretor                = "Bong Joon-ho",
                        Elenco                 = "Kang-ho Song, Sun-kyun Lee, Yeo-jeong Jo, Woo-sik Choi",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/ApiBzeaa95TNYliSbQ8pJv4Fje7.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/5xH0HfJHsaY",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/5xH0HfJHsaY",
                        CategoriaId            = catSuspense,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 16. A Origem (Inception) ─────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "A Origem",
                        AnoLancamento          = 2010,
                        Duracao                = 148,
                        Descricao              = "Um ladrão especialista em invadir os sonhos das pessoas para extrair informações valiosas de seu subconsciente recebe uma missão inversa: plantar uma ideia na mente de um CEO.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Tom Hardy",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/s3TBrRGB1iav7gFOCNx3H31MoES.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/YoHD9XEInc0",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/YoHD9XEInc0",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 17. O Poderoso Chefão ────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "O Poderoso Chefão",
                        AnoLancamento          = 1972,
                        Duracao                = 175,
                        Descricao              = "O patriarca envelhecido de uma dinastia do crime organizado transfere o controle de seu império clandestino para seu filho relutante.",
                        Diretor                = "Francis Ford Coppola",
                        Elenco                 = "Marlon Brando, Al Pacino, James Caan, Richard Castellano",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsLegHnDmni4v.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/tmU7GeKVybMWFButWEGl2M4GeiP.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/sY1S34973zA",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/sY1S34973zA",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 18. Clube da Luta ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Clube da Luta",
                        AnoLancamento          = 1999,
                        Duracao                = 139,
                        Descricao              = "Um narrador insone e um fabricante de sabão criam um clube de luta subterrâneo que evolui para algo muito mais perturbador.",
                        Diretor                = "David Fincher",
                        Elenco                 = "Brad Pitt, Edward Norton, Helena Bonham Carter, Meat Loaf",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/hZkgoQYus5vegHoetLkCJzb17zJ.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/qtRKdVHc-cE",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/qtRKdVHc-cE",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class18,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 19. Forrest Gump ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Forrest Gump",
                        AnoLancamento          = 1994,
                        Duracao                = 142,
                        Descricao              = "A presidência de vários presidentes, a guerra do Vietnã, o movimento pelos direitos civis e outros eventos históricos do século XX se desdobram através da perspectiva de um homem do Alabama com QI abaixo da média.",
                        Diretor                = "Robert Zemeckis",
                        Elenco                 = "Tom Hanks, Robin Wright, Gary Sinise, Sally Field",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/qdIMHd4sEfJSckfVJfKQvisL02a.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/bLvqoHBptjg",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/bLvqoHBptjg",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 20. Matrix ───────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Matrix",
                        AnoLancamento          = 1999,
                        Duracao                = 136,
                        Descricao              = "Um hacker descobre que toda a realidade que conhece é uma simulação criada por máquinas, e se junta a um grupo de rebeldes para lutar contra elas.",
                        Diretor                = "Lilly Wachowski, Lana Wachowski",
                        Elenco                 = "Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss, Hugo Weaving",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/fNG7i7RqMErkcqhohV2a6cV1Ehy.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/m8e-FF8MsqU",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/m8e-FF8MsqU",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 21. O Senhor dos Anéis: A Sociedade do Anel ─────────────────────────────
                    new Filme
                    {
                        Titulo                 = "O Senhor dos Anéis: A Sociedade do Anel",
                        AnoLancamento          = 2001,
                        Duracao                = 178,
                        Descricao              = "Um jovem hobbit chamado Frodo Bolseiro herda um anel misterioso que pode ser a chave para a dominação de toda a Terra Média pela força das trevas. Ele parte em uma jornada épica acompanhado por uma sociedade de companheiros.",
                        Diretor                = "Peter Jackson",
                        Elenco                 = "Elijah Wood, Ian McKellen, Orlando Bloom, Sean Connery",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/6oom5QYQ2yQTMJIbnvbkBL9cHo6.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/pIgMoFPGCMJ9axUJDaHDw0tB86m.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/V75dMMIW2B4",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/V75dMMIW2B4",
                        CategoriaId            = catAventura,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 22. O Senhor dos Anéis: O Retorno do Rei ─────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "O Senhor dos Anéis: O Retorno do Rei",
                        AnoLancamento          = 2003,
                        Duracao                = 201,
                        Descricao              = "A batalha final pelo destino da Terra Média se aproxima. Frodo e Samwise avançam em direção à Montanha da Perdição enquanto o Exército do Bem faz o último confronto contra as forças de Sauron.",
                        Diretor                = "Peter Jackson",
                        Elenco                 = "Elijah Wood, Viggo Mortensen, Ian McKellen, Orlando Bloom",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/rCzpDGLbOoPwLjy3OAm5NUPOTrC.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/lXhgCODAbBXL5buk9yEmTpOoOgR.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/r5X-hFf6Bwo",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/r5X-hFf6Bwo",
                        CategoriaId            = catAventura,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 23. Star Wars: O Despertar da Força ──────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Star Wars: O Despertar da Força",
                        AnoLancamento          = 2015,
                        Duracao                = 138,
                        Descricao              = "Três décadas após o Império, uma nova ameaça surge: a Primeira Ordem. Com a ajuda de uma jovem destemida, um ex-soldado e um velho amigo, os Rebeldes confrontam os novos vilões.",
                        Diretor                = "J.J. Abrams",
                        Elenco                 = "Harrison Ford, Mark Hamill, Carrie Fisher, Adam Driver",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/wqnLdwVXoBjKibFRR5U3y0aDUhs.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/c2Ax8Rox5g6CneChwy1idykaZdO.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/sGbxmsDFVnE",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/sGbxmsDFVnE",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class10,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 24. Homem-Aranha: Sem Volta para Casa ─────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Homem-Aranha: Sem Volta para Casa",
                        AnoLancamento          = 2021,
                        Duracao                = 148,
                        Descricao              = "Peter Parker pede ao Doutor Estranho que o mundo esqueça que ele é o Homem-Aranha. O feitiço atrai super-vilões de universos paralelos para o seu mundo.",
                        Diretor                = "Jon Watts",
                        Elenco                 = "Tom Holland, Zendaya, Benedict Cumberbatch, Willem Dafoe",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/iQFcwSGbZXMkeyKrxbPnwnRo5fl.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/JfVOs4VSpmA",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/JfVOs4VSpmA",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 25. Guardiões da Galáxia ──────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Guardiões da Galáxia",
                        AnoLancamento          = 2014,
                        Duracao                = 121,
                        Descricao              = "Um grupo improvável de anti-heróis se une para proteger uma esfera muito poderosa das garras do fanático Ronan, um vilão que ameaça o equilíbrio do universo.",
                        Diretor                = "James Gunn",
                        Elenco                 = "Chris Pratt, Vin Diesel, Bradley Cooper, Zoe Saldana",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/r7vmZjiyZw9rpJMQJdXpjgiCOk9.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/bHarw68REr6IURV0b4bOyveDdOH.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/d96cjJhvlMA",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/d96cjJhvlMA",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 26. Thor: Ragnarok ───────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Thor: Ragnarok",
                        AnoLancamento          = 2017,
                        Duracao                = 130,
                        Descricao              = "Thor é aprisionado no outro lado do universo. Para salvar Asgard de Hela, a deusa da morte, ele precisa lutar numa arena e vencer o Incrível Hulk.",
                        Diretor                = "Taika Waititi",
                        Elenco                 = "Chris Hemsworth, Tom Hiddleston, Cate Blanchett, Idris Elba",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/rzRwTcFvttcN1ZpX2znIiWMt8Wx.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/kaIfm5ryEOwmOqJYoMnWMQv40t5.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/ue80QwXMRHg",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/ue80QwXMRHg",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 27. Pantera Negra ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Pantera Negra",
                        AnoLancamento          = 2018,
                        Duracao                = 134,
                        Descricao              = "T'Challa retorna à nação africana de Wakanda para assumir o trono. Mas quando um inimigo do passado surge para desafiar seu poder, o Rei guerreiro se vê em um conflito que coloca todo o destino de Wakanda em risco.",
                        Diretor                = "Ryan Coogler",
                        Elenco                 = "Chadwick Boseman, Michael B. Jordan, Lupita Nyong'o, Danai Gurira",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/uxzzxijgPIY7slzFvMotPv8wjKA.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/b6ZJZHUdMEFECvGiDpJjlfUWela.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/xjDjIWPwcPU",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/xjDjIWPwcPU",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 28. Tenet ─────────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Tenet",
                        AnoLancamento          = 2020,
                        Duracao                = 150,
                        Descricao              = "Um agente secreto aprende a manipular o fluxo do tempo e usa essa habilidade para evitar a Terceira Guerra Mundial.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "John David Washington, Robert Pattinson, Elizabeth Debicki, Kenneth Branagh",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/k68nPLbIST6NP96JmTxmZijWhGb.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/wzJRB4MKi3yK138bpyqA2E2HIXR.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/LdOM0x0XDMo",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/LdOM0x0XDMo",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 29. Dunkirk ───────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Dunkirk",
                        AnoLancamento          = 2017,
                        Duracao                = 106,
                        Descricao              = "Soldados aliados britânicos, belgas, canadenses e franceses são cercados por forças alemãs e evacuados durante uma batalha decisiva da Segunda Guerra Mundial.",
                        Diretor                = "Christopher Nolan",
                        Elenco                 = "Fionn Whitehead, Tom Glynn-Carney, Jack Lowden, Harry Styles",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/ebSnODDg9lbsMIaWg2uAbjn7TO5.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/fudEG8VHJJEPi7QSBOQZ9j8WXsG.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/F-eMt3SrfFU",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/F-eMt3SrfFU",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 30. 1917 ─────────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "1917",
                        AnoLancamento          = 2019,
                        Duracao                = 119,
                        Descricao              = "April 6th, 1917. As a regiment assembles to wage war deep in enemy territory, two soldiers are assigned to race against time and deliver a message that will stop 1,600 men from walking straight into a deadly trap.",
                        Diretor                = "Sam Mendes",
                        Elenco                 = "George MacKay, Dean-Charles Chapman, Mark Strong, Andrew Scott",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/iZf0KyrE25z1sage4SYFLCCrMi9.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/yrFR7M2HpPb5oFpkqNz8FmXSS7e.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/YqNYrYUiMfg",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/YqNYrYUiMfg",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 31. Mad Max: Estrada da Fúria ─────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Mad Max: Estrada da Fúria",
                        AnoLancamento          = 2015,
                        Duracao                = 120,
                        Descricao              = "Em um mundo apocalíptico, o solitário Max se junta à imperatriz Furiosa para escapar do tirano Immortan Joe. Uma perseguição brutal no deserto que beira o impossível.",
                        Diretor                = "George Miller",
                        Elenco                 = "Tom Hardy, Charlize Theron, Nicholas Hoult, Hugh Keays-Byrne",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/8tZYtuWezp8JbcsvHYO0O46tFbo.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/phszHPFnhRUMhHHMoOgPtKRNiKz.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/hEJnMQG9ev8",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/hEJnMQG9ev8",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 32. Gravity ───────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Gravity",
                        AnoLancamento          = 2013,
                        Duracao                = 91,
                        Descricao              = "Uma engenheira médica em sua primeira missão espacial é lançada ao espaço profundo após um acidente. Ela deve sobreviver e encontrar seu caminho de volta à Terra.",
                        Diretor                = "Alfonso Cuarón",
                        Elenco                 = "Sandra Bullock, George Clooney, Ed Harris, Orto Ignatiussen",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/iJBraj7Nkl4RqBgguRQBz1VKXKM.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/hGFGOdSRBP4EQniRMFOCGBlSX6N.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/OiTiKOy59o4",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/OiTiKOy59o4",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 33. The Revenant: O Renascido ────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "The Revenant: O Renascido",
                        AnoLancamento          = 2015,
                        Duracao                = 156,
                        Descricao              = "Um explorador, atacado e deixado para morrer por seu guia, busca sobrevivência e vingança no selvagem oeste americano do século XIX.",
                        Diretor                = "Alejandro G. Iñárritu",
                        Elenco                 = "Leonardo DiCaprio, Tom Hardy, Will Poulter, Domhnall Gleeson",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/ji3ecJphATlVgq9BBk1J7L1YgwS.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/lijwRHkGrCbNQdS8kGXxDqJHAjx.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/LoebZZ8K5N0",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/LoebZZ8K5N0",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 34. Bohemian Rhapsody ────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Bohemian Rhapsody",
                        AnoLancamento          = 2018,
                        Duracao                = 134,
                        Descricao              = "A história de Freddie Mercury, o lendário vocalista do Queen, desde a formação da banda até a lendária apresentação no Live Aid em 1985.",
                        Diretor                = "Bryan Singer",
                        Elenco                 = "Rami Malek, Lucy Boynton, Gwilym Lee, Ben Hardy",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/lHu1wtNaczFPGFDTrjCSzeLPTKN.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/3vZDHryGCbOqgLh1Y5NcNY7OQVP.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/mP0VHJYFOAU",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/mP0VHJYFOAU",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 35. Whiplash ─────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Whiplash",
                        AnoLancamento          = 2014,
                        Duracao                = 107,
                        Descricao              = "Um jovem baterista ambicioso frequenta o melhor conservatório de música do país, onde é orientado por um dos regentes mais temidos do país.",
                        Diretor                = "Damien Chazelle",
                        Elenco                 = "Miles Teller, J.K. Simmons, Melissa Benoist, Paul Reiser",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/7fn624j5lj3xTme2SgiLCeuedmO.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/fRGxZuo7jJUWQsVg9PREb98Aclp.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/7d_jQycdQGo",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/7d_jQycdQGo",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 36. La La Land ───────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "La La Land",
                        AnoLancamento          = 2016,
                        Duracao                = 128,
                        Descricao              = "Enquanto Los Angeles vivencia outra bela estação, um ator e uma pianista de jazz se apaixonam ao mesmo tempo que buscam seus sonhos e lutam para achar um lugar em um mundo que raramente os acolhe.",
                        Diretor                = "Damien Chazelle",
                        Elenco                 = "Ryan Gosling, Emma Stone, John Legend, Rosemarie DeWitt",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/uDO8zWDhfWwoFdKS4fzkUJt0Rf0.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/nadTlnTE6DdgmYsN4iWb4gHwpDj.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/0pdqf4P9MB8",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/0pdqf4P9MB8",
                        CategoriaId            = catComedia,
                        ClassificacaoIndicativaId = class10,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 37. O Lobo de Wall Street ────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "O Lobo de Wall Street",
                        AnoLancamento          = 2013,
                        Duracao                = 180,
                        Descricao              = "Baseado na história real de Jordan Belfort, desde sua ascensão no mercado financeiro de Nova York até sua queda após um esquema de corrupção e crime organizado.",
                        Diretor                = "Martin Scorsese",
                        Elenco                 = "Leonardo DiCaprio, Jonah Hill, Margot Robbie, Kyle Chandler",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/pWHf4khOloNVfCxscsXFj3jj6gP.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/s3TBrRGB1iav7gFOCNx3H31MoES.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/iszwuX1AK6A",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/iszwuX1AK6A",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class18,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 38. Django Livre ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Django Livre",
                        AnoLancamento          = 2012,
                        Duracao                = 165,
                        Descricao              = "Com a ajuda de um caçador de recompensas alemão, um escravo recém-liberto percorre o sul dos Estados Unidos para resgatar sua esposa de um proprietário de plantação brutal.",
                        Diretor                = "Quentin Tarantino",
                        Elenco                 = "Jamie Foxx, Christoph Waltz, Leonardo DiCaprio, Kerry Washington",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/5WJnGM9GKgMqXkd2PeWxnBtjfqO.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/7Lh5R7tBpyGDPBDKHWxqlJbgFN.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/eUdM9vrCbow",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/eUdM9vrCbow",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 39. Bastardos Inglórios ───────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Bastardos Inglórios",
                        AnoLancamento          = 2009,
                        Duracao                = 153,
                        Descricao              = "Na França ocupada pelos nazistas durante a Segunda Guerra Mundial, o Tenente Aldo Raine organiza um grupo de soldados judeus para cometer atos violentos de vingança contra os nazistas.",
                        Diretor                = "Quentin Tarantino",
                        Elenco                 = "Brad Pitt, Christoph Waltz, Michael Fassbender, Eli Roth",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/vDwqPyhkzFPRDmwz9KbTN5jFxm3.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/7cBaQbBrFZJjfQKvdV3iy6Q9kxR.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/KnrRy6kSFFU",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/KnrRy6kSFFU",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 40. Era Uma Vez em Hollywood ─────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Era Uma Vez em Hollywood",
                        AnoLancamento          = 2019,
                        Duracao                = 161,
                        Descricao              = "Um ator e seu dublê navegam pelo mundo de Hollywood em seus últimos anos de glória durante o verão de 1969 em Los Angeles.",
                        Diretor                = "Quentin Tarantino",
                        Elenco                 = "Leonardo DiCaprio, Brad Pitt, Margot Robbie, Emile Hirsch",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/8j58iEBw9pOXFD2L0nt0ZXeHviB.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/aI7J7OP3EPbXBjAHnq8H5Sld71x.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/ELeMaP8EPAA",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/ELeMaP8EPAA",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 41. Jurassic World ───────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Jurassic World",
                        AnoLancamento          = 2015,
                        Duracao                = 124,
                        Descricao              = "Um novo parque temático de dinossauros é inaugurado com sucesso. Mas quando um dinossauro geneticamente modificado escapa, o caos se instala e a sobrevivência humana fica em xeque.",
                        Diretor                = "Colin Trevorrow",
                        Elenco                 = "Chris Pratt, Bryce Dallas Howard, Nick Robinson, Ty Simpkins",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/dkMD5qe2QiAMGhCrOFNnF3M5TGF.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/bqlJDTOtXDRHpMhOMDiGOSFNzl3.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/RFinNxS5KN4",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/RFinNxS5KN4",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 42. Titanic ───────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Titanic",
                        AnoLancamento          = 1997,
                        Duracao                = 194,
                        Descricao              = "Uma jovem de família nobre se apaixona por um artista humilde a bordo do navio mais famoso da história, o Titanic, cujo destino trágico já conhecemos.",
                        Diretor                = "James Cameron",
                        Elenco                 = "Leonardo DiCaprio, Kate Winslet, Billy Zane, Kathy Bates",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/kHXEpyfl6zqn8a6YuozZUujufXf.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/kVrqfYjkTdQ",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/kVrqfYjkTdQ",
                        CategoriaId            = catDrama,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 43. De Volta para o Futuro ───────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "De Volta para o Futuro",
                        AnoLancamento          = 1985,
                        Duracao                = 116,
                        Descricao              = "Marty McFly é acidentalmente enviado de volta para 1955 em um carro-máquina do tempo DeLorean. Ele deve garantir que seus pais se apaixonem e encontrar uma maneira de voltar para o futuro.",
                        Diretor                = "Robert Zemeckis",
                        Elenco                 = "Michael J. Fox, Christopher Lloyd, Lea Thompson, Crispin Glover",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/fNOH9f1aA7XRTzl1sAOx9iF553Q.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/dziRPFmCFRHKDFcOJLjfIOOXSKO.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/qvsgGtivCgs",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/qvsgGtivCgs",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = classLivre,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 44. Gladiador ─────────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Gladiador",
                        AnoLancamento          = 2000,
                        Duracao                = 155,
                        Descricao              = "Um general romano traído e escravizado busca vingança contra o corrupto imperador que assassinou sua família. Sua jornada o transforma em um gladiador implacável.",
                        Diretor                = "Ridley Scott",
                        Elenco                 = "Russell Crowe, Joaquin Phoenix, Connie Nielsen, Oliver Reed",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/6WBIzCgmDCYrqh64yDREGeDk9d3.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/P5ieIbInFpg",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/P5ieIbInFpg",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class16,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 45. Missão: Impossível — Protocolo Fantasma ───────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Missão: Impossível — Protocolo Fantasma",
                        AnoLancamento          = 2011,
                        Duracao                = 133,
                        Descricao              = "O agente Ethan Hunt e sua equipe são responsabilizados por um atentado ao Kremlin. Para limpar o nome da IMF, eles devem agir sozinhos e impedir uma guerra nuclear.",
                        Diretor                = "Brad Bird",
                        Elenco                 = "Tom Cruise, Jeremy Renner, Simon Pegg, Paula Patton",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/s1cxSaH8gFuXGGFyFVNOhlXaHkv.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/g3jLiGLCVVkiHFbtAl8NQHuJiMB.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/I4sqAuzpB-w",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/I4sqAuzpB-w",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 46. Capitã Marvel ─────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Capitã Marvel",
                        AnoLancamento          = 2019,
                        Duracao                = 124,
                        Descricao              = "Carol Danvers se torna uma das heroínas mais poderosas do universo enquanto a Terra é apanhada no meio de uma guerra galáctica entre duas raças alienígenas.",
                        Diretor                = "Anna Boden, Ryan Fleck",
                        Elenco                 = "Brie Larson, Samuel L. Jackson, Ben Mendelsohn, Jude Law",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/AtsgWhDnHTq68L0lLsUrCnM7TjG.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/w2PMyoyLU22YvrGK3smVM9fW1jj.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/Z1BCujX3pw8",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/Z1BCujX3pw8",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 47. Rápidos e Furiosos 7 ─────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Rápidos e Furiosos 7",
                        AnoLancamento          = 2015,
                        Duracao                = 137,
                        Descricao              = "Deckard Shaw busca vingança pelo irmão contra Dominic Toretto e sua equipe enquanto um misterioso mercenário usa a equipe como peões em sua missão de matar um hacker.",
                        Diretor                = "James Wan",
                        Elenco                 = "Vin Diesel, Paul Walker, Dwayne Johnson, Jason Statham",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/dIWDFAMfGdPCabkOvxaGCbFNWRF.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/qWb7GjHHMBSHEp1gBbkGH5wRuSb.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/Skpu5HaKkmw",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/Skpu5HaKkmw",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 48. Jurassic Park ────────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Jurassic Park",
                        AnoLancamento          = 1993,
                        Duracao                = 127,
                        Descricao              = "Durante uma visita prévia a um parque temático de dinossauros criados por engenharia genética, um sabotador desliga os sistemas de segurança e os dinossauros fogem e começam a caçar os visitantes.",
                        Diretor                = "Steven Spielberg",
                        Elenco                 = "Sam Neill, Laura Dern, Jeff Goldblum, Richard Attenborough",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/oU7Oq2kZm5cCGjRl3kfBnFXcjdA.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/9BBTo108Kh4ROUNtSiVdNAZFCTa.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/QWBKEmWWL38",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/QWBKEmWWL38",
                        CategoriaId            = catAventura,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 49. Vingadores: Guerra Infinita ──────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Vingadores: Guerra Infinita",
                        AnoLancamento          = 2018,
                        Duracao                = 149,
                        Descricao              = "Os Vingadores e seus aliados devem estar dispostos a sacrificar tudo em uma tentativa de derrotar o poderoso Thanos, antes que sua blitz de devastação e ruína coloque um fim ao universo.",
                        Diretor                = "Anthony Russo, Joe Russo",
                        Elenco                 = "Robert Downey Jr., Chris Hemsworth, Mark Ruffalo, Chris Evans",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/7WsyChQLEftFiDOVTGkv3hFpyyt.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/lmZFxXgJE3vgrciwuDib0N8CfQo.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/6ZfuNTqbHE8",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/6ZfuNTqbHE8",
                        CategoriaId            = catAcao,
                        ClassificacaoIndicativaId = class12,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    },

                    // ── 50. Blade Runner 2049 ────────────────────────────────────────────────────
                    new Filme
                    {
                        Titulo                 = "Blade Runner 2049",
                        AnoLancamento          = 2017,
                        Duracao                = 164,
                        Descricao              = "Um novo caçador de andróides (K) descobre um segredo há muito soterrado que tem o potencial de mergulhar o que resta da sociedade no caos. A descoberta de K o leva a uma busca por Rick Deckard, um antigo caçador que desapareceu há trinta anos.",
                        Diretor                = "Denis Villeneuve",
                        Elenco                 = "Ryan Gosling, Harrison Ford, Ana de Armas, Sylvia Hoeks",
                        ImagemCapaUrl          = "https://image.tmdb.org/t/p/w500/gajva2L0rPYkEWjzgFlBXCAVBE5.jpg",
                        ImagemBannerUrl        = "https://image.tmdb.org/t/p/w1280/ilRyazdMJwN3LBXF64ZFhqOCsYH.jpg",
                        TrailerYoutubeUrl      = "https://www.youtube.com/embed/gCcx85zbxz4",
                        VideoYoutubeUrl        = "https://www.youtube.com/embed/gCcx85zbxz4",
                        CategoriaId            = catFiccao,
                        ClassificacaoIndicativaId = class14,
                        Ativo                  = true,
                        DataCadastro           = DateTime.UtcNow
                    }
                };

                context.Filmes.AddRange(filmes);
                context.SaveChanges();
            }

            // ================================================================
            // SEED DE FAVORITOS DE EXEMPLO (cliente padrao favorita 3 filmes)
            // ================================================================
            var clientePadrao = await userManager.FindByEmailAsync("cliente@senacflix.com");
            if (clientePadrao != null && !context.Favoritos.Any())
            {
                // Favorita os 3 primeiros filmes como exemplo para o cliente padrao
                var primeirosFilmes = context.Filmes.Take(3).ToList();
                foreach (var filme in primeirosFilmes)
                {
                    context.Favoritos.Add(new Favorito
                    {
                        UsuarioId    = clientePadrao.Id,
                        FilmeId      = filme.Id,
                        DataFavorito = DateTime.UtcNow
                    });
                }
                context.SaveChanges();
            }
        }

        // ================================================================
        // METODO AUXILIAR: Cria Roles e Usuarios padrao via Identity
        // Este metodo e idempotente: nao cria duplicatas se ja existirem.
        // ================================================================
        private static async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Cria os perfis (roles) se nao existirem
            string[] perfis = { "Admin", "Operador", "Cliente" };
            foreach (var perfil in perfis)
            {
                if (!await roleManager.RoleExistsAsync(perfil))
                    await roleManager.CreateAsync(new IdentityRole(perfil));
            }

            // ── Administrador padrao ────────────────────────────────────
            var adminExistente = await userManager.FindByEmailAsync("admin@senacflix.com");
            if (adminExistente == null)
            {
                var admin = new ApplicationUser
                {
                    UserName       = "admin@senacflix.com",
                    Email          = "admin@senacflix.com",
                    NomeCompleto   = "Administrador SenacFlix",
                    EmailConfirmed = true,
                    Ativo          = true
                };
                var resultado = await userManager.CreateAsync(admin, "Senac@123");
                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
            else if (!adminExistente.Ativo)
            {
                adminExistente.Ativo = true;
                await userManager.UpdateAsync(adminExistente);
                var token = await userManager.GeneratePasswordResetTokenAsync(adminExistente);
                await userManager.ResetPasswordAsync(adminExistente, token, "Senac@123");
            }

            // ── Operador padrao ─────────────────────────────────────────
            var operadorExistente = await userManager.FindByEmailAsync("operador@senacflix.com");
            if (operadorExistente == null)
            {
                var operador = new ApplicationUser
                {
                    UserName       = "operador@senacflix.com",
                    Email          = "operador@senacflix.com",
                    NomeCompleto   = "Operador SenacFlix",
                    EmailConfirmed = true,
                    Ativo          = true
                };
                var resultado = await userManager.CreateAsync(operador, "Senac@123");
                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(operador, "Operador");
            }
            else if (!operadorExistente.Ativo)
            {
                operadorExistente.Ativo = true;
                await userManager.UpdateAsync(operadorExistente);
                var token = await userManager.GeneratePasswordResetTokenAsync(operadorExistente);
                await userManager.ResetPasswordAsync(operadorExistente, token, "Senac@123");
            }

            // ── Cliente padrao ──────────────────────────────────────────
            var clienteExistente = await userManager.FindByEmailAsync("cliente@senacflix.com");
            if (clienteExistente == null)
            {
                var cliente = new ApplicationUser
                {
                    UserName       = "cliente@senacflix.com",
                    Email          = "cliente@senacflix.com",
                    NomeCompleto   = "Cliente SenacFlix",
                    EmailConfirmed = true,
                    Ativo          = true
                };
                var resultado = await userManager.CreateAsync(cliente, "Senac@123");
                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(cliente, "Cliente");
            }
            else if (!clienteExistente.Ativo)
            {
                clienteExistente.Ativo = true;
                await userManager.UpdateAsync(clienteExistente);
                var token = await userManager.GeneratePasswordResetTokenAsync(clienteExistente);
                await userManager.ResetPasswordAsync(clienteExistente, token, "Senac@123");
            }
        }
    }
}
