using AmigoSecretoAPI.Context;
using AmigoSecretoAPI.Repositories;
using AmigoSecretoAPI.Repositories.Interface;
using AmigoSecretoAPI.Services;
using AmigoSecretoAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AmigoSecretoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Registro dos repositórios
builder.Services.AddScoped<IGrupoRepository, GrupoRepository>();
builder.Services.AddScoped<IParticipanteRepository, ParticipanteRepository>();

// Registro dos serviços
builder.Services.AddScoped<IAmigoSecretoService, AmigoSecretoService>();

// Configuração do JSON para evitar ciclos de objetos
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura para lidar com ciclos de objetos
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
