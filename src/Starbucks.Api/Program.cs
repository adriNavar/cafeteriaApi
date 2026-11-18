using Starbucks.Api.Extensions;
using Starbucks.Persistence;
using Starbucks.Application;
using Core.Mappy.Interfaces;
using Starbucks.Application.Categories.DTOs;
using Core.Mappy.Extensions;
using Starbucks.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;

builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

// 1. REGISTRAR SERVICIOS DE SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.RegisterMappings(typeof(CategoryMappingProfile).Assembly);

await app.ApplyMigration(environment);

// 2. MIDDLEWARES DE SWAGGER HABILITADOS EN TODOS LOS ENTORNOS
app.UseSwagger(); // Genera el JSON en /swagger/v1/swagger.json
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentacion de Cafeteria");
    //option.RoutePrefix = string.Empty; // Carga Swagger directamente en la raíz "/"
});
app.UseCors("AllowAll");

app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();