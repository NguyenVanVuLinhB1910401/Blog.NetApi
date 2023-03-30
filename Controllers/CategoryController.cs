using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiBlog.Models;

namespace WebApiBlog.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public CategoryController(ILogger<CategoryController> logger, UserManager<AppUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("/api/Category/GetAll/{idUser}")]
        //[Authorize]
        public async Task<IActionResult> GetAllCategories(string? idUser)
        {
            if (_context.Categories != null)
            {
                var categories = await _context.Categories.Where(c => c.UserId == idUser).ToListAsync();
                return Ok(new { statusCode = 200, data = categories });
            }
            else
            {
                return NoContent();
            }

        }

        [HttpGet("/api/Category/GetOne/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetOneCategory(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.Include(c => c.User).Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);

        }

        [HttpPost("/api/Category/AddCategory")]
        [Authorize]
        public async Task<IActionResult> AddCategory([Bind("Title", "Description", "UserId")] Category category)
        {
            if (ModelState.IsValid)
            {
                // var claimsIdentity = this.User.Identity as ClaimsIdentity;
                // var UserId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                // category.UserId = UserId;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return Ok(category);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpDelete("/api/Category/DeleteCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            await _context.SaveChangesAsync();
            return Ok(new
            {
                statusCode = 200,
                message = "Delete success"
            });
        }

    }
}