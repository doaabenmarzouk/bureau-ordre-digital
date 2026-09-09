using Microsoft.EntityFrameworkCore;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BureauOrdreDigital.Api.Shared.Services;
using BureauOrdreDigital.Api.Modules.Assistant.Services;
using System.Runtime.ExceptionServices;
using BureauOrdreDigital.Api.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);


var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Connection string 'DefaultConnection' not found."
        );

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);


// =====================================================
// SERVICES
// =====================================================

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUtilisateurService, UtilisateurService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ICourrierService, CourrierService>();
builder.Services.AddScoped<IHistoriqueService, HistoriqueService>();

builder.Services.AddScoped<
    IOrganismeService,
    OrganismeService
>();

// Importateur Excel
builder.Services.AddScoped<OrganismeExcelImporter>();

builder.Services.AddScoped<IStockageService, StockageService>();
builder.Services.AddScoped<ITypeCourrierService, TypeCourrierService>();


// =====================================================
// SERVICE IA
// =====================================================

builder.Services.AddHttpClient<IAssistantIaService, AssistantIaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8000/");
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// =====================================================
// CORS
// =====================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5174",
                "http://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// =====================================================
// AUTHENTICATION
// =====================================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
            };
    });


// =====================================================
// AUTHORIZATION
// =====================================================

builder.Services.AddAuthorization();


// =====================================================
// BUILD APPLICATION
// =====================================================

var app = builder.Build();


// =====================================================
// IMPORT EXCEL DES ORGANISMES
// =====================================================

using (var scope = app.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

    var importer =
        scope.ServiceProvider
            .GetRequiredService<OrganismeExcelImporter>();

    if (!await db.Organismes.AnyAsync())
    {
        var cheminExcel = Path.Combine(
            app.Environment.ContentRootPath,
            "Imports",
            "Copie de NeoLedge - MEA-PU - Parc 2026 1.xlsx"
        );

        var nombreImporte =
            await importer.ImporterAsync(cheminExcel);

        Console.WriteLine(
            $"IMPORT EXCEL : {nombreImporte} organismes importés."
        );
    }
    else
    {
        Console.WriteLine(
            "IMPORT EXCEL : la table Organismes contient déjà des données."
        );
    }
}


app.UseMiddleware<ExceptionHandlingMiddleware>();


// =====================================================
// SWAGGER
// =====================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// =====================================================
// HTTPS
// =====================================================

app.UseHttpsRedirection();


// =====================================================
// CORS
// IMPORTANT : AVANT Authentication / Authorization
// =====================================================

app.UseCors("Frontend");


// =====================================================
// AUTHENTICATION
// =====================================================

app.UseAuthentication();


// =====================================================
// AUTHORIZATION
// =====================================================

app.UseAuthorization();


// =====================================================
// CONTROLLERS
// =====================================================

app.MapControllers();


// =====================================================
// HOME
// =====================================================

app.MapGet(
    "/",
    () => "Bureau d'Ordre Digital API"
);


// =====================================================
// DEBUG ROUTES
// =====================================================

var actionProvider =
    app.Services.GetRequiredService<
        Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider
    >();

foreach (var route in actionProvider.ActionDescriptors.Items)
{
    Console.WriteLine(
        $"[DEBUG] Controller trouvé : {route.DisplayName}"
    );
}


// =====================================================
// RUN
// =====================================================

app.Run();