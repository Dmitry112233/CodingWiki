using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingWiki_Web.Controllers;

public class PublisherController : Controller
{
    private ApplicationDbContext _db;
    
    public PublisherController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }
    
    public IActionResult Index()
    {
        var publishers = _db.Publishers.ToList();
        return View(publishers);
    }

    
    public IActionResult Upsert(int? id)
    {
        Publisher publisher = new();
        if (id == 0 || id == null)
        {
            return View(publisher);
        }

        publisher = _db.Publishers.FirstOrDefault(i => i.PublisherId == id);
        if (publisher == null)
        {
            return NotFound();
        }
        return View(publisher);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upsert(Publisher obj)
    {
        if (ModelState.IsValid)
        {
            if (obj.PublisherId == 0)
            {
                await _db.Publishers.AddAsync(obj);
            }
            else
            {
                _db.Publishers.Update(obj);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(obj);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var publisher = _db.Publishers.FirstOrDefault(i => i.PublisherId == id);
        if (publisher == null)
        {
            return NotFound();
        }

        _db.Publishers.Remove(publisher);
        await _db.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
}