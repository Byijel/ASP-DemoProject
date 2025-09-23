using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ap.Demo.Infrastructure.Contexts
{
    public class MyCitiesContextFactory : IDesignTimeDbContextFactory<MyCitiesContext>
    {
        public MyCitiesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyCitiesContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyCitiesDb;Trusted_Connection=True;");

            return new MyCitiesContext(optionsBuilder.Options);
        }
    }
}