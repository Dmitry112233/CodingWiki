// See https://aka.ms/new-console-template for more information

using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;

Console.WriteLine("Hello, World!");

//AddBook();
//GetAllBooks();
//GetBook(1001);

/*void GetAllBooks()
{
    using var context = new ApplicationDbContext();
    var books = context.Books.ToList();
    foreach (var book in books)
    {
        Console.WriteLine(book.Title + " - " + book.ISBN);
    }
}

void GetBook(int id)
{
    using var context = new ApplicationDbContext();
    var book = context.Books.FirstOrDefault(u => u.BookId == id); 
    Console.WriteLine(book.Title + " - " + book.ISBN);
}

void AddBook()
{
    using var context = new ApplicationDbContext();
    context.Books.Add(new Book(){Title = "Heroes", Price = 8.1m, ISBN = "123B13", PublisherId = 2});
    context.SaveChanges();
}
*/


