using Microsoft.EntityFrameworkCore;
using BureauOrdreDigital.Api.Shared.Data;
using BureauOrdreDigital.Api.Modules.Utilisateur.Services;
using BureauOrdreDigital.Api.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddControllers();
 builder.Services.AddEndpointsApiExplorer();
 builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
