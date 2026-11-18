using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Identity;
using Azure.Core;

namespace Starbucks.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<StarbucksDbContext>(opt =>
        {
            var connectionString = configuration.GetConnectionString("SqlAzureDataBase");

            var sqlConnection = new SqlConnection(connectionString);

            // Si estamos en entorno de desarrollo local o ejecutando con credenciales de VS, 
            // podemos inyectar el token. En Azure App Service la infraestructura gestiona el token nativamente,
            // pero asegurar el objeto SqlConnection evita conflictos.
            try
            {
                var credential = new DefaultAzureCredential();
                var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
                var accessToken = credential.GetToken(tokenRequestContext);
                sqlConnection.AccessToken = accessToken.Token;
            }
            catch
            {
                // Si falla localmente por credenciales, dejará que use la cadena base o fallará controlado
            }

            opt.UseSqlServer(
                sqlConnection,
                sqlOptions => sqlOptions.EnableRetryOnFailure()
            ).UseSnakeCaseNamingConvention();
        });

        return services;
    }
}