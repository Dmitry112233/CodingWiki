using CodingWiki_Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodingWiki_DataAccess.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<BookDetail> BookDetails { get; set; }
    
    public DbSet<BookAuthorMap> BookAuthorMaps { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*optionsBuilder.UseSqlServer("Server=127.0.0.1, 1431;Database=sql-coding-wiki;TrustServerCertificate=True;Trusted_Connection=false;MultipleActiveResultSets=true;User Id=SA;Password=MyPass@word")
            .LogTo(Console.WriteLine, new [] {DbLoggerCategory.Database.Command.Name}, LogLevel.Information);*/
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Book>().Property(u => u.Price).HasPrecision(10, 5);
        modelBuilder.Entity<BookAuthorMap>().HasKey(u => new { u.AuthorId, u.BookId });

        var books = new List<Book>()
        {
            new () { BookId = 1, Title = "Spider", ISBN = "123B12", Price = 7.99m, PublisherId = 1},
            new () { BookId = 2, Title = "Bug", ISBN = "123B12", Price = 6.11m, PublisherId = 2},
            new () { BookId = 3, Title = "Pussy", ISBN = "123B12", Price = 11.72m, PublisherId = 3 },
            new () { BookId = 4, Title = "Dota2 for claw-shaped", ISBN = "123B12", Price = 100, PublisherId = 1 }
        };

        modelBuilder.Entity<Book>().HasData(books);
        
        var publishers = new List<Publisher>()
        {
            new () { PublisherId = 1, Name = "Sipaniny Production", Location = "London"},
            new () { PublisherId = 2, Name = "Vella Go", Location = "Rim"},
            new () { PublisherId = 3, Name = "Sisi", Location = "Moscow"}
        };
        
        modelBuilder.Entity<Publisher>().HasData(publishers);
        
        var categories = new List<Category>()
        {
            new () { CategoryId = 1, CategoryName = "Cat 1"},
            new () { CategoryId = 2, CategoryName = "Cat 2"},
            new () { CategoryId = 3, CategoryName = "Cat 3"}
        };
        
        modelBuilder.Entity<Category>().HasData(categories);
    }
}