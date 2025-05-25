using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using LogAnalizerServer.Data;

namespace LogAnalizerServer
{
    public class LogAnalizerServerDbContextFactory : IDesignTimeDbContextFactory<LogAnalizerServerDbContext>
    {
        public LogAnalizerServerDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var server = configuration.GetSection("DatabaseSettings")["Server"];
            var database = configuration.GetSection("DatabaseSettings")["Database"];

            var connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";

            var optionsBuilder = new DbContextOptionsBuilder<LogAnalizerServerDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new LogAnalizerServerDbContext(optionsBuilder.Options);
        }
    }
}
