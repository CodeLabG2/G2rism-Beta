using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace G2rismBeta.API.Data;

/// <summary>
/// Factory para crear el DbContext en tiempo de diseño (Design-Time)
/// Esto es necesario para que los comandos de EF Core funcionen correctamente
/// Ejemplo: dotnet ef migrations add, dotnet ef database update
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Crear configuración para leer appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Obtener la connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Configurar el DbContext con MySQL
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
