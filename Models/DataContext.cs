namespace Models
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        /* public schemes */

        // public DbSet<BusinessType> BusinessType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Vehicle>().ToTable("Vehicle", "administrative");

            /* Limita y retringue eliminación en cascada */
            foreach (
                var relationship in modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(e => e.GetForeignKeys())
            )
            {
                relationship.DeleteBehavior = relationship.IsRequired
                    ? DeleteBehavior.Restrict
                    : DeleteBehavior.SetNull;
            }
        }
    }
}
