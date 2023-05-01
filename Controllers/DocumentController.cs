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




}
