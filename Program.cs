
using DSW_3P_PR03.Repositories;
using DSW_3P_PR03.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores.
builder.Services.AddControllers();

// Configurar Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar repositorio JSON.
builder.Services.AddScoped<IPersonajeRepository, PersonajeJsonRepository>();

// Registrar servicio.
builder.Services.AddScoped<IPersonajeService, PersonajeService>();

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
