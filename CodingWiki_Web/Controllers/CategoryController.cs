using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodingWiki_Web.Controllers;

public class CategoryController : Controller
{
    private ApplicationDbContext _db;
    
    public CategoryController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IActionResult Index()
    {
        var categories = _db.Categories.ToList();
        return View(categories);
    }

    
    public IActionResult Upsert(int? id)
    {
        Category category = new();
        if (id == 0 || id == null)
        {
            return View(category);
        }

        category = _db.Categories.FirstOrDefault(i => i.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upsert(Category obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.CategoryId == 0)
            {
                await _db.Categories.AddAsync(obj);
            }
            else
            {
                _db.Categories.Update(obj);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(obj);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var category = _db.Categories.FirstOrDefault(i => i.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateMultiple2()
    {
        for (int i = 0; i < 2; i++)
        {
            await _db.Categories.AddAsync(new Category
            {
                CategoryName = Guid.NewGuid().ToString()
            });
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> CreateMultiple5()
    {
        for (int i = 0; i < 5; i++)
        {
            await _db.Categories.AddAsync(new Category
            {
                CategoryName = Guid.NewGuid().ToString()
            });
        }

        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> RemoveMultiple2()
    {
        IEnumerable<Category> categories = _db.Categories.OrderByDescending(u => u.CategoryId).Take(2);
        _db.RemoveRange(categories);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult>RemoveMultiple5()
    {
        IEnumerable<Category> categories = _db.Categories.OrderByDescending(u => u.CategoryId).Take(5);
        _db.RemoveRange(categories);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}