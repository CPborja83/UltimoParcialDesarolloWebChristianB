using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UltimoParcialDesarolloWebChristianB.Repository;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllers();

// Registrar tu repositorio Cines (la clase que tiene los métodos de manejo de base de datos)
builder.Services.AddScoped<Cines>();  // Usamos tu clase Cines.cs de la carpeta Repository

// Swagger para la documentación de tu API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();  // Redirigir HTTP a HTTPS

app.UseAuthorization();
app.UseDefaultFiles(); // Usa el archivo por defecto como "index.html"
app.UseStaticFiles();
app.MapControllers();  // Mapea los controladores para la API

app.Run();
