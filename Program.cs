using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using DSW_3P_PR03.Data;
using DSW_3P_PR03.Repositories;
using DSW_3P_PR03.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores.
builder.Services.AddControllers();

// Configurar Swagger.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                new List<string>()
        });
});

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

// Configurar JWT.
var jwt = builder.Configuration.GetSection("Jwt");

var key = Convert.FromBase64String(
    jwt["Key"] ?? throw new InvalidOperationException(
        "Falta configurar Jwt:Key"
    )
);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Configurar CORS.
string[] origenes = builder.Configuration
    .GetSection("Cors:Origins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CharacterHubFrontend", policy =>
    {
        policy.WithOrigins(origenes)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();
app.UseCors("CharacterHubFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
