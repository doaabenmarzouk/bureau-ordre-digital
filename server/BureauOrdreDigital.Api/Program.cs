using Microsoft.EntityFrameworkCore;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using BureauOrdreDigital.Api.Modules.Courrier.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BureauOrdreDigital.Api.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================================================
// DATABASE
// =====================================================

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
    IServiceDestinataireService,
    ServiceDestinataireService
>();

builder.Services.AddScoped<IStockageService, StockageService>();
builder.Services.AddScoped<ITypeCourrierService, TypeCourrierService>();


// =====================================================
// CONTROLLERS + SWAGGER
// =====================================================

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
// JWT AUTHENTICATION
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

app.MapGet("/", () => "Bureau d'Ordre Digital API");


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