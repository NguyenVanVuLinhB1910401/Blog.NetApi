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
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public PostController(ILogger<PostController> logger, UserManager<AppUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("/api/Post/GetAllPostsOfCategory/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetAllPosts(int? id)
        {
            if (_context.Posts != null && id != null)
            {
                var posts = await _context.Posts.Where(p => p.CategoryId == id).ToListAsync();
                return Ok(posts);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpGet("/api/Post/GetOnePost/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetOnePost(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.Include(p => p.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);

        }



        [HttpPost("/api/Post/AddPost")]
        //[Authorize]
        public async Task<IActionResult> AddPost([Bind("Title", "Description", "CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreateAt = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return Ok(post);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpDelete("/api/Post/DeletePost/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (_context.Categories == null || id == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}