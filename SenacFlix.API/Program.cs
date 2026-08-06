using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SenacFlix.Application;
using SenacFlix.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// CONFIGURAÇÃO DE SERVIÇOS E INJEÇÃO DE DEPENDÊNCIAS
// ===================================================

// 1. Registra os serviços da infraestrutura (DbContext, Identity, Jwt)
builder.Services.AdicionarServicosDeInfraestrutura(builder.Configuration);

// 2. Registra os serviços da Aplicação(Serviços, AutoMapper)
builder.Services.AdicionarServicosDeAplicacao();

// 3. Configura a Autenticação via JWT
var chaveJwt = builder.Configuration["Jwt:Chave"] ?? "SenacFlixChaveSecretaSuperSegura2024!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt)),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Emissor"],
        ValidateAudience = true,
        ClockSkew = TimeSpan.Zero,
    };
});

// 4. Configura a Autorização
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// 5. Configura o CORS para permitir requisições do MVC e Desktop
builder.Services.AddCors(options =>
{
    options.AddPolicy("SenacFlixCors", policy =>
    {
        policy.WithOrigins("http://localhost:5002", "http://localhost:5000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
// 6. Configura o Swagger (Documentação da API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SenacFlix API",
        Version = "v1",
        Description = "API REST da plataforma de streaming SenacFlix",
        Contact = new OpenApiContact { Name = "Suporte SenacFlix", Email = "suporte@senacflix.com" }
    });

    // Configuração para o Swagger aceitar o token JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT retornado no login. Exemplo: 'eyJhbGci...'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

var app = builder.Build();

// ==========================================================
// CONFIGURAÇÃO DO PIPELINE DE REQUISIÇÃO (MIDDLEWARES)
// ==========================================================

// Removido SeedDados pois os dados iniciais vêm via HasData no EF Migrations

// Habilita o Swagger e SwaggerUI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SenacFlix API V1");
    c.RoutePrefix = string.Empty; // Define o Swagger como a página inicial
});

// Adiciona o redirecionamento para HTTPS para que o aplicativo seja acessado por HTTPS
app.UseHttpsRedirection();
// Adiciona o CORS para que o aplicativo seja acessado por diferentes origens
app.UseCors("SenacFlixCors");

// Autenticação e Autorização devem ser chamados nesta ordem exata
app.UseAuthentication();
app.UseAuthorization();

// Adiciona o mapeamento dos controladores para que a API possa responder às requisições
app.MapControllers();











//builder.Services.AddSwaggerGen(c =>
//{

//});

app.Run();
