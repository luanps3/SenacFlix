// Nome do arquivo: SenacFlixContexto.cs
// Objetivo: Contexto principal do Entity Framework Core da aplicacao SenacFlix.
//           Representa a "ponte" entre o codigo C# e o banco de dados SQL Server.
//           Contem todos os DbSets (tabelas) e as configuracoes de relacionamentos.
// Camada: Infrastructure
// Como participa: E injetado nos repositorios via Dependency Injection.
//                 E responsavel por todas as operacoes de leitura e escrita no banco.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SenacFlix.Domain.Entidades;

namespace SenacFlix.Infrastructure.Dados
{
    // SenacFlixContexto herda de IdentityDbContext<ApplicationUser>
    // Isso significa que o EF(Entity Framework) gerencia automaticamente as tabelas do Identity
    // (usuairos, roles, claims, etc...) além das nossas tabelas customizadas
    public class SenacFlixContexto : IdentityDbContext<ApplicationUser>
    {
        //Construtor que recebe as opções de configuração do DbContext
        // As opcoes vem da injeção de dependência (configurado no Program.cs via
        // DI(Dependency Injection))
        public SenacFlixContexto(DbContextOptions<SenacFlixContexto> opcoes) 
            : base(opcoes)// Repassa as opções para a classe pai(IdentityDbContext)
        {
        }

        // DbSet representa uma tabela no banco de dados
        // Cada propriedade aqui gera uma tabela correspondente via migration
        public DbSet<Filme> Filmes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Favorito> Favoritos { get; set; } = null!;
        public DbSet<Auditoria> Auditorias { get; set; } = null!;
        public DbSet<ClassificacaoIndicativa> ClassificacoesIndicativas { get; set; } = null!;



        // OnModelCreating e chamado pelo EF Core ao criar o modelo do banco de dados
        // Aqui configuramos relacionamentos, restricoes e comportamentos usando Fluent API
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANTE: sempre chamar o metodo da classe pai primeiro
            // Isso garante que as tabelas do Identity (usuarios, roles, etc.) sejam configuradas
            base.OnModelCreating(builder);

            // ================================================================
            // CONFIGURACAO DA ENTIDADE FILME
            // ================================================================

            builder.Entity<Filme>(entidade =>
            {
                // Define o nome da tabela no banco de dados
                entidade.ToTable("Filmes");

                // Titulo e obrigatorio e tem limite de 200 caracteres
                entidade.Property(f => f.Titulo)
                    .IsRequired()
                    .HasMaxLength(200);

                // Descricao e obrigatoria
                entidade.Property(f => f.Descricao)
                    .IsRequired();

                // Diretor tem limite de 150 caracteres
                entidade.Property(f => f.Diretor)
                    .HasMaxLength(150);

                // ImagemCapaUrl nao e obrigatoria (pode ser null)
                entidade.Property(f => f.ImagemCapaUrl)
                    .HasMaxLength(500);

                // ImagemBannerUrl nao e obrigatoria
                entidade.Property(f => f.ImagemBannerUrl)
                    .HasMaxLength(500);

                // URLs do YouTube nao sao obrigatorias
                entidade.Property(f => f.TrailerYoutubeUrl)
                    .HasMaxLength(500);

                entidade.Property(f => f.VideoYoutubeUrl)
                    .HasMaxLength(500);

                // Configuracao do relacionamento Filme -> Categoria (muitos para um)
                // Um filme pertence a uma categoria, uma categoria pode ter varios filmes
                entidade.HasOne(f => f.Categoria)
                    .WithMany(c => c.Filmes)
                    .HasForeignKey(f => f.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict); // Nao permite deletar categoria com filmes vinculados

                // Configuracao do relacionamento Filme -> ClassificacaoIndicativa (muitos para um)
                entidade.HasOne(f => f.ClassificacaoIndicativa)
                    .WithMany()
                    .HasForeignKey(f => f.ClassificacaoIndicativaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ================================================================
            // CONFIGURACAO DA ENTIDADE CATEGORIA
            // ================================================================

            builder.Entity<Categoria>(entidade =>
            {
                entidade.ToTable("Categorias");

                // Nome da categoria e obrigatorio e tem maximo de 100 caracteres
                entidade.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(100);

                // Descricao e opcional
                entidade.Property(c => c.Descricao)
                    .HasMaxLength(500);
            });

            // ================================================================
            // CONFIGURACAO DA ENTIDADE CLASSIFICACAO INDICATIVA
            // ================================================================

            builder.Entity<ClassificacaoIndicativa>(entidade =>
            {
                entidade.ToTable("ClassificacoesIndicativas");

                // Nome tem limite de 20 caracteres (ex: "18 anos")
                entidade.Property(c => c.Nome)
                    .IsRequired()
                    .HasMaxLength(20);

                // Descricao opcional
                entidade.Property(c => c.Descricao)
                    .HasMaxLength(300);

                // Cor armazena um valor CSS como "#FF0000" (maximo 20 caracteres)
                entidade.Property(c => c.Cor)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            // ================================================================
            // CONFIGURACAO DA ENTIDADE FAVORITO
            // ================================================================

            builder.Entity<Favorito>(entidade =>
            {
                entidade.ToTable("Favoritos");

                // Configuracao do relacionamento Favorito -> ApplicationUser
                // UsuarioId e uma string pois o Identity usa string como chave primaria
                entidade.HasOne(f => f.Usuario)
                    .WithMany()
                    .HasForeignKey(f => f.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade); // Se o usuario for deletado, seus favoritos tambem sao

                // Configuracao do relacionamento Favorito -> Filme
                entidade.HasOne(f => f.Filme)
                    .WithMany(fi => fi.Favoritos)
                    .HasForeignKey(f => f.FilmeId)
                    .OnDelete(DeleteBehavior.Cascade); // Se o filme for deletado, os favoritos tambem sao

                // Garante que um usuario nao pode favoritar o mesmo filme duas vezes
                // Cria um indice unico composto por UsuarioId + FilmeId
                entidade.HasIndex(f => new { f.UsuarioId, f.FilmeId })
                    .IsUnique();
            });

            // ================================================================
            // CONFIGURACAO DA ENTIDADE AUDITORIA
            // ================================================================

            builder.Entity<Auditoria>(entidade =>
            {
                entidade.ToTable("Auditorias");

                // Acao e obrigatoria (ex: "Cadastrou Filme", "Editou Categoria")
                entidade.Property(a => a.Acao)
                    .IsRequired()
                    .HasMaxLength(200);

                // TabelaAfetada e obrigatoria (ex: "Filmes", "Categorias")
                entidade.Property(a => a.TabelaAfetada)
                    .IsRequired()
                    .HasMaxLength(100);

                // NomeUsuario e opcional (pode nao estar disponivel em alguns casos)
                entidade.Property(a => a.NomeUsuario)
                    .HasMaxLength(200);

                // Detalhes pode ser um texto longo descrevendo o que foi alterado
                entidade.Property(a => a.Detalhes)
                    .HasMaxLength(2000);
            });

            // ================================================================
            // RENOMEAR TABELAS DO IDENTITY PARA MELHOR ORGANIZACAO
            // ================================================================
            // Por padrao o Identity cria tabelas com nomes como "AspNetUsers".
            // Aqui renomeamos para um padrao mais organizado com prefixo "Identidade_"

            builder.Entity<ApplicationUser>().ToTable("Identidade_Usuarios");
            builder.Entity<IdentityRole>().ToTable("Identidade_Perfis");
            builder.Entity<IdentityUserRole<string>>().ToTable("Identidade_UsuarioPerfis");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Identidade_UsuarioClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Identidade_UsuarioLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("Identidade_PerfilClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("Identidade_UsuarioTokens");

            // ================================================================
            // APPLY SEED DATA
            // ================================================================
            // Os dados iniciais agora sao injetados nativamente via SQL na migration SeedData.
        }



    }
}
