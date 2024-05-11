using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using CodingWiki_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodingWiki_Web.Controllers;

public class BookController : Controller
{
    private ApplicationDbContext _db;
    
    public BookController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IActionResult Index()
    {
        List<Book> books = _db.Books
            .Include(u => u.Publisher)
            .Include(u => u.BookAuthorMap)
            .ThenInclude(u => u.Author).ToList();
        
        /*var books = _db.Books.ToList();
        foreach (var book in books)
        {
            _db.Entry(book).Reference(u => u.Publisher).Load();
            _db.Entry(book).Collection(u => u.BookAuthorMap).Load();
            foreach (var bookAuth in book.BookAuthorMap)
            {
                _db.Entry(bookAuth).Reference(u => u.Author).Load();
            }
        }*/
        return View(books);
    }
    
    public IActionResult Details(int? id)
    {
        if (id == 0 || id == null)
        {
            return NotFound();
        }
        
        var obj = _db.BookDetails.Include(u => u.Book).FirstOrDefault(u => u.BookId == id);

        if (obj == null)
        {
            obj = new BookDetail();
            obj.Book = _db.Books.FirstOrDefault(i => i.BookId == id);
        }
        
        if (obj.Book != null) 
            obj.BookId = obj.Book.BookId;

        return View(obj);
    }

    [HttpPost]
    public async Task<IActionResult> Details(BookDetail obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.BookDetailId == 0)
            {
                obj.Book = _db.Books.FirstOrDefault(i => i.BookId == obj.BookId);
                await _db.BookDetails.AddAsync(obj);
            }
            else
            {
                _db.BookDetails.Update(obj);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        return View(obj);
    }

    public IActionResult ManageAuthors(int id)
    {
        BookAuthorVM obj = new()
        {
            BookAuthorList = _db.BookAuthorMaps.Include(u => u.Author).Include(u => u.Book)
                .Where(u => u.BookId == id).ToList(),
            BookAuthor = new ()
            {
                BookId = id
            },
            Book = _db.Books.FirstOrDefault(u => u.BookId == id)
        };

        List<int> tempListOfAssignedAuthor = obj.BookAuthorList.Select(u => u.AuthorId).ToList();
        var tempList = _db.Authors.Where(u => !tempListOfAssignedAuthor.Contains(u.AuthorId)).ToList();
        obj.AuthorList = tempList.Select(i => new SelectListItem
        {
            Text = i.FullName,
            Value = i.AuthorId.ToString()
        });

        return View(obj);
    }
    
    [HttpPost]
    public async Task<IActionResult> ManageAuthors(BookAuthorVM bookAuthorVm)
    {
        if (bookAuthorVm.BookAuthor.BookId != 0 && bookAuthorVm.BookAuthor.AuthorId != 0)
        {
            await _db.BookAuthorMaps.AddAsync(bookAuthorVm.BookAuthor);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("ManageAuthors", new { @id = bookAuthorVm.BookAuthor.BookId });
    }
    
    public async Task<IActionResult> RemoveAuthors(int authorId, BookAuthorVM bookAuthorVm)
    {
        int bookId = bookAuthorVm.Book.BookId;
        var bookAuthorMap = _db.BookAuthorMaps.FirstOrDefault(i => i.AuthorId == authorId && i.BookId == bookId);
        
        if (bookAuthorMap == null)
        {
            return NotFound();
        }

        _db.BookAuthorMaps.Remove(bookAuthorMap);
        await _db.SaveChangesAsync();

        return RedirectToAction("ManageAuthors", new {@id = bookId });
    }
    
    public IActionResult Upsert(int? id)
    {
        BookVM bookVm = new BookVM();
        bookVm.PublisherList = _db.Publishers.Select(i 
            => new SelectListItem { Text = i.Name, Value = i.PublisherId.ToString() }).ToList();
        
        if (id == 0 || id == null)
        {
            return View(bookVm);
        }

        bookVm.Book = _db.Books.FirstOrDefault(i => i.BookId == id);
        if (bookVm.Book == null)
        {
            return NotFound();
        }
        return View(bookVm);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert(BookVM obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.Book.BookId == 0)
            {
                await _db.Books.AddAsync(obj.Book);
            }
            else
            {
                _db.Books.Update(obj.Book);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        return View(obj);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = _db.Books.FirstOrDefault(i => i.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}