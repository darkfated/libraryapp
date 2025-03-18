using Microsoft.EntityFrameworkCore;

namespace libraryapp
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public LibraryContext() { }
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite("Data Source=library.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
                entity.Property(b => b.PublishYear).IsRequired();
                entity.Property(b => b.QuantityInStock).IsRequired();
                entity.HasOne(b => b.Author).WithMany(a => a.Books).HasForeignKey(b => b.AuthorId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(b => b.Genre).WithMany(g => g.Books).HasForeignKey(b => b.GenreId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.BirthDate).IsRequired();
                entity.Property(a => a.Country).HasMaxLength(100);
            });
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).IsRequired().HasMaxLength(100);
                entity.Property(g => g.Description).HasMaxLength(500);
            });
        }
    }
}
