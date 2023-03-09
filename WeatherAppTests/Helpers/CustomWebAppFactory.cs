using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WeatherApp.Contexts;

namespace WeatherAppTests.Helpers
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        // This method creates a custom host for the test application.
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // Creates an in-memory SQLite database connection for testing.
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            builder.ConfigureServices(services =>
            {
                // Removes the existing database context options so that the
                // in-memory SQLite database can be used instead.
                services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));

                // Adds the application database context with the in-memory
                // SQLite database connection to the dependency injection container.
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });
            });

            // Creates the host with the modified service configuration.
            return base.CreateHost(builder);
        }

        // This method is called when the test host is disposed of.
        protected override void Dispose(bool disposing)
        {
            // Closes the database connection when the host is disposed of.
            //_connection.Close();

            // Calls the base Dispose method to clean up other resources.
            base.Dispose(disposing);
        }
    }
}
