using Microsoft.AspNetCore.Authentication.Cookies;
using SenacFlix.UI.Infraestrutura;
using SenacFlix.UI.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona o serviço de acesso ao contexto HTTP
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Conta/Login";
        option.LogoutPath = "/Conta/Sair";
        option.AccessDeniedPath = "/Conta/AcessoNegado";
        option.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("SenacFlixAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiConfiguracoes:UrlBase"] ?? "http://localhost:5031");
})
    //Ignora validação de certificado SSL apenas para ambiente de desenvolvimento local
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

builder.Services.AddScoped<ApiCliente>();
builder.Services.AddScoped<ServicoUpload>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // o HSTS serve para informar aos navegadores que o
    // site deve ser acessado apenas via HTTPS,
    // evitando ataques de downgrade e cookies inseguros.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Autenticação tem que vir antes de autorização , senão não funciona.
app.UseAuthentication();
app.UseAuthorization();

//Mapeamento das rotas com Areas (Admin, Cliente)
//e padrão de rota para controllers sem Area.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
