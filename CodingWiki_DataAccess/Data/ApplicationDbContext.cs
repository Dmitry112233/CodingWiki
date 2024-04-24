using CodingWiki_Model.Models;
using Microsoft.EntityFrameworkCore;

namespace CodingWiki_DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=127.0.0.1, 1431;Database=sql-coding-wiki;TrustServerCertificate=True;Trusted_Connection=false;MultipleActiveResultSets=true;User Id=SA;Password=MyPass@word");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>().Property(u => u.Price).HasPrecision(10, 5);

        var books = new List<Book>()
        {
            new () { BookId = 1, Title = "Spider", ISBN = "123B12", Price = 7.99m },
            new () { BookId = 2, Title = "Bug", ISBN = "123B12", Price = 6.11m },
            new () { BookId = 3, Title = "Pussy", ISBN = "123B12", Price = 11.72m },
            new () { BookId = 4, Title = "Dota2 for claw-shaped", ISBN = "123B12", Price = 100 }
        };

        modelBuilder.Entity<Book>().HasData(books);
    }
}