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

namespace E_ranga.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Namespace.IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;


    public HomeController(ILogger<HomeController> logger, Namespace.IUserRepository userRepository, ApplicationDbContext context)
    {
        _logger = logger;
        _userRepository = userRepository;
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLogin loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                bool passwordMatchesHash = BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.Password);
                if (passwordMatchesHash)
                {
                    HttpContext.Session.SetString("email", loginViewModel.Email);
                    return RedirectToAction("Dashboard", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Password");
                }
            }
            else
            {
                Console.WriteLine("Invalid email or password");
                ModelState.AddModelError("", "Invalid email or password");
            }
        }

        return View(loginViewModel);
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
            _context.documents.Add(doc);
            _context.SaveChanges();
            TempData["success"] = " Thank you for posting the lost document. Your contribution is greatly appreciated!";
            return RedirectToAction("Index");
        }
        return View("Create", doc);
    }
[HttpGet]
public IActionResult ViewDocument()
{
    try
    {
        var documents = _context.documents.OrderBy(d => d.Status).ToList();
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
        return View(documents);
    }
    catch (System.Exception)
    {
        throw;
    }
}

}

