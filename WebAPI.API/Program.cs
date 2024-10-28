using Microsoft.OpenApi.Models;
using WebAPI.Implementation;
using WebAPI.Interface;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Servicios
builder.Services.AddControllers();
builder.Services.AddScoped<IDispositivoService, DispositivoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProductoService, ProductoService>();


// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Dispositivos", Version = "v1" });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Dispositivos V1");
        c.RoutePrefix = string.Empty; // Hacer que Swagger UI esté disponible en la raíz
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
