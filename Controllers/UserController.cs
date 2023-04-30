using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using E_ranga.Data;
using Microsoft.EntityFrameworkCore;
using Namespace;
using Microsoft.AspNetCore.Identity;

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
                var passwordHasher = new PasswordHasher<UserRegister>();
                var hashedPassword = passwordHasher.HashPassword(users, users.Password);
                var newUser = new UserRegister
                {
                    FirstName = users.FirstName,
                    LastName = users.LastName,
                    Email = users.Email,
                    Address = users.Address,
                    PhoneNumber = users.PhoneNumber,
                    Password = hashedPassword
                };
                await _userRepository.CreateUserAsync(newUser);
                return RedirectToAction("Login");
            }
            else{
            ModelState.AddModelError(string.Empty, "Email already exists");
            }
        }
        Console.WriteLine("Email Already Exist");
        return View("Register", users);
    }
      [HttpGet]
      public IActionResult Dashboard()
        {
            return View();
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
            Console.WriteLine(user.Email);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<UserLogin>();
                var result = passwordHasher.VerifyHashedPassword(loginViewModel, user.Password, loginViewModel.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    // Login successful
                    // You can set a session variable or some other means of tracking the logged in user here
                  Console.WriteLine("Logged in Sucessfully!!");
                    return RedirectToAction("Dashboard");
                }
            }
            else{
            Console.WriteLine("Invalid email or password");
            ModelState.AddModelError("", "Invalid email or password");
            }
        }

        return View(loginViewModel);
    }
    }
}
