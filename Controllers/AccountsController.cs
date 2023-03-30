using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApiBlog.Models;

namespace WebApiBlog.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountsController(ILogger<AccountsController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.UserName);
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.UserName),
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidAudience"],
                    audience: _configuration["JWT:ValidIssuer"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha256Signature)
                );
                user.PasswordHash = null;
                var response = new
                {
                    statusCode = 200,
                    message = "Đăng nhập thành công",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user = user
                };
                return Ok(response);
            }

            return Ok(new
            {
                statusCode = 401,
                message = "Sai email hoặc mật khẩu",
            });

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) return Ok(StatusCode(201));
            else return Unauthorized();
        }
    }
}