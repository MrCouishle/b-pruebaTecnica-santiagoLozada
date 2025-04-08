namespace Models
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<RouletteNumber> RouletteNumber { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<RouletteNumber>()
                .HasData(
                    new RouletteNumber { Id = 1, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 2, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 3, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 4, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 5, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 6, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 7, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 8, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 9, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 10, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 11, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 12, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 13, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 14, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 15, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 16, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 17, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 18, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 19, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 20, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 21, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 22, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 23, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 24, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 25, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 26, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 27, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 28, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 29, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 30, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 31, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 32, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 33, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 34, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 35, Color = ColorEnums.Black },
                    new RouletteNumber { Id = 36, Color = ColorEnums.Red },
                    new RouletteNumber { Id = 37, Color = ColorEnums.Green }
                );

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
