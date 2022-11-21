global using Microsoft.EntityFrameworkCore;

namespace CRUDMinimalAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Superhero> Superheroes => Set<Superhero>();
    }
}
