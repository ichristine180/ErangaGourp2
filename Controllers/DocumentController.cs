using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using E_ranga.Data;

namespace E_ranga.Controllers;

public class DocumentController : Controller
{
    private readonly ILogger<DocumentController> _logger;
    private readonly ApplicationDbContext _context;
    public DocumentController(ILogger<DocumentController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> Publish(int Id)
    {
        await UpdateStatus(Id, 1,"The Document has been Unpublished successfully!");
        return RedirectToAction("Dashboard", "User");
    }

    [HttpGet]
    public async Task<IActionResult> Unpublish(int Id)
    {
        await UpdateStatus(Id, 2,"The Document has been Unpublished successfully!");
        return RedirectToAction("Dashboard", "User");
    }
    [HttpGet]
    public IActionResult Delete(int Id)
    {

        return RedirectToAction("Dashboard", "User");
    }

    public async Task UpdateStatus(int id, int status,string message)
    {
        var existingDoc = await _context.documents.FindAsync(id);

        if (existingDoc == null)
        {
            return;
        }

        existingDoc.Status = status;

        _context.documents.Update(existingDoc);
        await _context.SaveChangesAsync();

        TempData["success"] = message;
    }

}
