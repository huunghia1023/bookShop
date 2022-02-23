using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace bookShopSolution.Data.EF
{
    public class BookShopSolutionDbContextFactory : IDesignTimeDbContextFactory<BookShopDbContext>
    {
        public BookShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("bookShopDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<BookShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BookShopDbContext(optionsBuilder.Options);
        }
    }
}
