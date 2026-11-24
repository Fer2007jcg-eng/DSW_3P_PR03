using Microsoft.EntityFrameworkCore;
using DSW_3P_PR03.Data;
using DSW_3P_PR03.Repositories;
using DSW_3P_PR03.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores.
builder.Services.AddControllers();

// Configurar Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar repositorio SQL.
builder.Services.AddScoped<IPersonajeRepository, PersonajeSqlRepository>();

// Registrar servicio.
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

// Configurar SQL Server.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    )
);

var app = builder.Build();

// Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
