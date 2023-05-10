using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using E_ranga.Data;
using Microsoft.EntityFrameworkCore;

namespace E_ranga.Controllers;

public class DocumentController : BaseController
{
    private readonly ILogger<DocumentController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly Namespace.IUserRepository _userRepository;
    public DocumentController(ILogger<DocumentController> logger, ApplicationDbContext context, Namespace.IUserRepository userRepository)
    {
        _logger = logger;
        _context = context;
        _userRepository = userRepository;
    }


    [HttpGet]
    public async Task<IActionResult> Publish(int Id)
    {
        await UpdateStatus(Id, 1, "The Document has been Published successfully!");
        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> Unpublish(int Id)
    {
        await UpdateStatus(Id, 2, "The Document has been Unpublished successfully!");
        return RedirectToAction("Dashboard");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _context.documents.FindAsync(id);
        if (doc == null)
        {
            return NotFound();
        }

        _context.documents.Remove(doc);
        await _context.SaveChangesAsync();

        TempData["success"] = "Document deleted successfully!";
        return RedirectToAction("Dashboard");
    }

    public async Task UpdateStatus(int id, int status, string message)
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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Documents doc, IFormFile image)
    {
        if (ModelState.IsValid)
        {
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    doc.ImageData = memoryStream.ToArray();
                }
            }
            var user = await _userRepository.GetUserByEmailAsync(HttpContext.Session.GetString("email"));
            if (user.Email != null)
            {
                doc.PosterId = user.Id;
                _context.documents.Add(doc);
                _context.SaveChanges();
                TempData["success"] = " Thank you for posting the lost document. Your contribution is greatly appreciated!";
                return RedirectToAction("Index", "Home");
            }

        }
        return View("Create", doc);
    }
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        try
        {
            string email = HttpContext.Session.GetString("email");
            ViewData["email"] = email;
            if (email != null)
            {
                var documents = _context.documents.Include(v => v.UserRegister).OrderBy(d => d.Status).ToList();
                return View(documents);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    public async Task<IActionResult> UserM()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return View(users);
    }

    public IActionResult GetImage(int id)
    {
        var doc = _context.documents.FirstOrDefault(d => d.Id == id);
        if (doc == null || doc.ImageData == null)
        {
            return NotFound();
        }
        return File(doc.ImageData, "image/jpeg");
    }

}
