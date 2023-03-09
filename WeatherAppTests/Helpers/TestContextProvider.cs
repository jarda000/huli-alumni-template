using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using WeatherApp.Contexts;

namespace WeatherAppTests.Helpers
{
    public class TestContextProvider
    {
        public static ApplicationDbContext CreateContextFromScratch()
        {
            var connection = new SqliteConnection("Datasource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;
            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static ApplicationDbContext CreateContextFromFactory(CustomWebAppFactory factory)
        {
            var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var connection = context.Database.GetDbConnection();
            connection.Close();
            connection.Open();

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
