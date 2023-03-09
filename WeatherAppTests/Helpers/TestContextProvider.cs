using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using WeatherApp.Contexts;

namespace WeatherAppTests.Helpers
{
    public class TestContextProvider
    {
        // Creates a new instance of ApplicationDbContext using an in-memory SQLite database.
        public static ApplicationDbContext CreateContextFromScratch()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();

            // Create a new instance of DbContextOptions using the SQLite connection.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

            // Create a new instance of ApplicationDbContext using the DbContextOptions.
            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static ApplicationDbContext CreateContextFromFactory(CustomWebAppFactory factory)
        {
            // Create a new service scope for the factory.
            var scope = factory.Services.CreateScope();

            // Get the ApplicationDbContext instance from the service scope.
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Get the underlying database connection from the ApplicationDbContext instance.
            var connection = context.Database.GetDbConnection();

            connection.Close();
            connection.Open();

            // Ensure that the database has been deleted and re-created.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        public static Mock<ApplicationDbContext> CreateEmptyMockContext()
        {
            return new Mock<ApplicationDbContext>();
        }
    }
}
