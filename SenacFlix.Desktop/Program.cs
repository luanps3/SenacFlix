// Nome do arquivo: Program.cs
// Objetivo: Inicializacao do Windows Forms com Injeção de Dependência
// Camada: Desktop

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenacFlix.Desktop.Forms;
using SenacFlix.Desktop.Infraestrutura;
using System;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace SenacFlix.Desktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Configura o host generico para DI e configuracao
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(AppContext.BaseDirectory);
                    builder.AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices((context, services) =>
                {
                    // Configura HttpClient para a API
                    services.AddHttpClient("SenacFlixAPI", client =>
                    {
                        var baseUrl = context.Configuration["ApiConfiguracoes:UrlBase"] ?? "http://localhost:5031";
                        client.BaseAddress = new Uri(baseUrl);
                    })
                    // Ignora certificado SSL em dev
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
                    });

                    // Registra servicos e forms
                    services.AddSingleton<ApiClienteDesktop>();
                    services.AddSingleton<SenacFlix.Desktop.ApiClientes.PerfilApiCliente>();
                    services.AddTransient<FormLogin>();
                    services.AddTransient<FormPrincipal>();
                })
                .Build();

            var loginForm = host.Services.GetRequiredService<FormLogin>();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                var formPrincipal = host.Services.GetRequiredService<FormPrincipal>();
                Application.Run(formPrincipal);
            }
        }
    }
}