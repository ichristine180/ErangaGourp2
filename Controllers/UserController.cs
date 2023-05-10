using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using E_ranga.Data;
using Namespace;
using Microsoft.EntityFrameworkCore;
namespace E_ranga.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegister users)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(users.Email);
                if (user == null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(users.Password);
                    var newUser = new UserRegister
                    {
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        Email = users.Email,
                        Address = users.Address,
                        PhoneNumber = users.PhoneNumber,
                        Password = hashedPassword,
                        Role = "user"
                    };
                    await _userRepository.CreateUserAsync(newUser);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email already exists");
                }
            }
            return View("Register", users);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
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
                        HttpContext.Session.SetString("role", user.Role);
                        HttpContext.Session.SetString("email", loginViewModel.Email);
                        if (user.Role == "admin")
                            return RedirectToAction("Dashboard", "Document");
                        else return RedirectToAction("Create", "Document");
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


    }


}
