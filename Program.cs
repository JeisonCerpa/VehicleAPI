using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VehicleAPI.Configuration;
using VehicleAPI.Data;
using VehicleAPI.Models;
using VehicleAPI.Services;
using VehicleAPI.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging temprano para diagnóstico de configuración
var logger = LoggerFactory.Create(config => 
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Information);
}).CreateLogger("Program");

// Cargar variables de entorno desde .env
try 
{
    // Configurar opciones para cargar variables de entorno
    var envFilePath = Path.Combine(builder.Environment.ContentRootPath, ".env");
    
    // Crear opciones directamente en lugar de usar lambda
    var options = new DotNetEnv.LoadOptions(
        setEnvVars: true,
        clobberExistingVars: true,
        onlyExactPath: false
    );
    
    var envVars = Env.Load(envFilePath, options);
    
    var fileExists = File.Exists(envFilePath);
    logger.LogInformation("Archivo .env {Status}", fileExists ? "cargado" : "no encontrado");
}
catch (Exception ex)
{
    logger.LogWarning("Error al cargar archivo .env: {Error}", ex.Message);
}

// Configurar ajustes fuertemente tipados desde appsettings.json
var configSection = builder.Configuration.GetSection("VehicleSettings");
builder.Services.Configure<VehicleSettings>(configSection);

// Leer variables críticas con prioridad a variables de entorno
var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULTCONNECTION") ?? 
                       builder.Configuration.GetConnectionString("DefaultConnection");

// Validar configuración crítica
if (string.IsNullOrEmpty(connectionString))
{
    var message = "CONFIGURACIÓN CRÍTICA FALTANTE: Cadena de conexión no configurada. " +
                  "Configure CONNECTIONSTRINGS__DEFAULTCONNECTION o DefaultConnection en appsettings.json";
    logger.LogCritical(message);
    throw new InvalidOperationException(message);
}

logger.LogInformation("Configuración aplicada con éxito. Base de datos configurada.");

builder.Services.AddDbContext<VehicleDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Register services with proper dependency injection
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IVehicleDataFormatterService, VehicleDataFormatterService>();
builder.Services.AddScoped<ICriticalFieldsNotificationService, CriticalFieldsNotificationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Validar configuración de correo electrónico
using (var scope = app.Services.CreateScope())
{
    var scopedLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var settings = scope.ServiceProvider.GetRequiredService<IOptions<VehicleSettings>>().Value;
    
    // Verificar configuración de correo
    if (string.IsNullOrEmpty(settings.Email?.Usuario) || string.IsNullOrEmpty(settings.Email?.Contraseña))
    {
        scopedLogger.LogWarning("Configuración de email incompleta. Las notificaciones por correo podrían no funcionar.");
    }
    
    // Verificar configuración de alertas
    if (string.IsNullOrEmpty(settings.Alerta?.Destinatario))
    {
        scopedLogger.LogWarning("Destinatario de alertas no configurado. Las alertas se enviarán a una dirección predeterminada.");
    }
    
    scopedLogger.LogInformation("Iniciando aplicación con la configuración cargada");
}

// Aplicar migraciones automáticamente
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
        dbContext.Database.Migrate();
        logger.LogInformation("Migraciones de base de datos aplicadas con éxito");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "Error al aplicar migraciones de base de datos");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Usar el puerto de Render si está definido, o 80 por defecto
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
app.Urls.Add($"http://0.0.0.0:{port}");
app.Run();
