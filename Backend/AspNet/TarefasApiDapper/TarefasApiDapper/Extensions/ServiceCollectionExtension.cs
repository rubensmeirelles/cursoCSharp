using System.Data.SqlClient;
using static TarefasApiDapper.Data.TarefaContext;

namespace TarefasApiDapper.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddScoped<GetConnection>(sp => async () =>
            {
                var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            });
            return builder;
        }
    }
}
