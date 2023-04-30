using Microsoft.AspNetCore.Mvc;
using E_ranga.Models;
using E_ranga.Data;
using BCrypt.Net;
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
                    Password = hashedPassword
                };
                await _userRepository.CreateUserAsync(newUser);
                return RedirectToAction("Login");
            }
            else{
            ModelState.AddModelError(string.Empty, "Email already exists");
            }
        }
        return View("Register", users);
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
                    // Login successful
                    // You can set a session variable or some other means of tracking the logged in user here
                  Console.WriteLine("Logged in Sucessfully!!");
                  if(loginViewModel.Email == "admin@gmail.com"){
                    return RedirectToAction("Dashboard");
                  }else{
                    return RedirectToAction("Index", "Home");
                  }
                }else{
                    ModelState.AddModelError("", "Invalid Password");
                }
            }
            else{
            Console.WriteLine("Invalid email or password");
            ModelState.AddModelError("", "Invalid email or password");
            }
        }

        return View(loginViewModel);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(UserRegister user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _userRepository.UpdateUserAsync(user);
                return RedirectToAction("Dashboard");
            }
            catch (DbUpdateConcurrencyException)
            {
                var userExist = await _userRepository.GetUserByEmailAsync(user.Email);
                if (userExist == null)
                {
                    return NotFound();
                }
                throw;
            }
        }
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _userRepository.DeleteUserAsync(user);
        return RedirectToAction("Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return View(users);
    }
    }
}
