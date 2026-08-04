// Nome do arquivo: DependencyInjection.cs
// Objetivo: Configurar a Injeção de Dependencia da camada de Infraestrutura
// Camada: Infrastructure
// Como participa: Registra os repositorios, o DbContext e o Identity para serem usados na API

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SenacFlix.Domain.Entidades;
using SenacFlix.Domain.Interfaces;
using SenacFlix.Infrastructure.Dados;
using SenacFlix.Infrastructure.Repositorios;

namespace SenacFlix.Infrastructure
{
    public static class InfrastructureExtensoes
    {
        public static IServiceCollection AdicionarServicosDeInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra o DbContext
            services.AddDbContext<SenacFlixContexto>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SenacFlixDB")));

            // Registra o Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<SenacFlixContexto>()
            .AddDefaultTokenProviders();

            // Registra os repositorios
            services.AddScoped<IFilmeRepositorio, FilmeRepositorio>();
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IFavoritoRepositorio, FavoritoRepositorio>();
            services.AddScoped<IAuditoriaRepositorio, AuditoriaRepositorio>();
            services.AddScoped<IClassificacaoRepositorio, ClassificacaoRepositorio>();
            services.AddScoped<IEstatisticasRepositorio, EstatisticasRepositorio>();

            return services;
        }
    }
}
