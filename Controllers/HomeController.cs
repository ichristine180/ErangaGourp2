using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using BCrypt.Net;
using E_ranga.Data;
using Npgsql;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
namespace E_ranga.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
  
    private readonly ApplicationDbContext _context;


    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public void convertImage(List<Documents> documents)
    {
        foreach (var doc in documents)
        {
            // Convert image data from byte array to image format
            if (doc.ImageData != null && doc.ImageData.Length > 0)
            {
                var imageBase64Data = Convert.ToBase64String(doc.ImageData);
                var imageDataUrl = string.Format("data:image/png;base64,{0}", imageBase64Data);
                doc.ImageDataUrl = imageDataUrl;
            }
        }
    }
    public List<Documents> GetDocuments()
    {
        var documents =  _context.documents.Include(v => v.UserRegister).OrderBy(d => d.Status).Where(d => d.Status == 1).ToList();
        convertImage(documents);
        return documents;
    }
    public IActionResult Index()
    {
        try
        {
            var documents = GetDocuments();
            return View(documents);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Search(string searchTerm)
    {
        try
        {
            var documents = GetDocuments();
            if (searchTerm != null)
            {
                documents = _context.documents.Include(v => v.UserRegister).Where(p => p.OwnerNames.ToLower().Contains(searchTerm.ToLower()) || p.DocType.ToLower().Contains(searchTerm.ToLower()) && p.Status == 1).ToList();
                convertImage(documents);
            }
            return View("ViewDocument", documents);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    
}

