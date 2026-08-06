// Nome do arquivo: DependencyInjection.cs
// Objetivo: Classe de extensao para registro de dependencias da camada Application
// Camada: Application
// Como participa: Facilita a configuracao do container de DI no Program.cs da API

using Microsoft.Extensions.DependencyInjection;
using SenacFlix.Application.Mapeamentos;
using SenacFlix.Application.Servicos.Implementacoes;
using SenacFlix.Application.Servicos.Interfaces;

namespace SenacFlix.Application
{
    public static class ApplicationExtensoes
    {
        public static IServiceCollection AdicionarServicosDeAplicacao(this IServiceCollection services)
        {
            // Registra os perfis do AutoMapper
            services.AddAutoMapper(cfg => cfg.AddProfile<PerfilMapeamento>());

            // Registra os servicos da aplicacao
            services.AddScoped<IFilmeServico, FilmeServico>();
            services.AddScoped<ICategoriaServico, CategoriaServico>();
            services.AddScoped<IFavoritoServico, FavoritoServico>();
            services.AddScoped<IAuditoriaServico, AuditoriaServico>();
            services.AddScoped<IClassificacaoServico, ClassificacaoServico>();
            services.AddScoped<IEstatisticasServico, EstatisticasServico>();

            return services;
        }
    }
}
