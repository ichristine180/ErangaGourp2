using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using BCrypt.Net;
using E_ranga.Data;
using Npgsql;


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
      public async Task<IActionResult> Create(Documents doc)
    {
        // if (ModelState.IsValid)
        // {
        //     _context.documents.Add(doc);
        //     _context.SaveChanges();
        //     TempData["ResultOk"] = "Document posted Successfully !";
        //     return RedirectToAction("Index");
        // }
        string connectionString = "Server=localhost;Port=5432;Database=eranga;User Id=postgres;Password=post123;";

        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        
            connection.Open();

            string sql = "INSERT INTO document (ownerName, posterName, poster MobileNo, documentType, documentImage, description) VALUES (@onames, @pnames, @pphone, @doctype, @docdata, @descr)";
            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            
                command.Parameters.AddWithValue("@onames", "owne_names");
                command.Parameters.AddWithValue("@pname", "poster_names");
                command.Parameters.AddWithValue("@pphone", "poster_phone");
                command.Parameters.AddWithValue("@doctype", "doc_type");
                command.Parameters.AddWithValue("@docdata", "doc_data");
                command.Parameters.AddWithValue("@descr", "status");

                command.ExecuteNonQuery();
                connection.Close();

    
       // Console.WriteLine("Not valid");
        return View("Create",doc);
    }
}

