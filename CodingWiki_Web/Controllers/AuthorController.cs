using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingWiki_Web.Controllers;

public class AuthorController : Controller
{
    private ApplicationDbContext _db;
    
    public AuthorController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IActionResult Index()
    {
        var authors = _db.Authors.ToList();
        return View(authors);
    }

    
    public IActionResult Upsert(int? id)
    {
        Author author = new();
        if (id == 0 || id == null)
        {
            return View(author);
        }

        author = _db.Authors.FirstOrDefault(i => i.AuthorId == id);
        if (author == null)
        {
            return NotFound();
        }
        return View(author);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upsert(Author obj)
    {
        TryValidateModel(obj);
        if (ModelState.IsValid)
        {
            if (obj.AuthorId == 0)
            {
                await _db.Authors.AddAsync(obj);
            }
            else
            {
                _db.Authors.Update(obj);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        return View(obj);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var author = _db.Authors.FirstOrDefault(i => i.AuthorId == id);
        if (author == null)
        {
            return NotFound();
        }

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}