using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TrendEmber.Core.Identity;
using TrendEmber.Service;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        { 
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized();
            }
            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { Token = token });
        }
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new {Message = "Logged out successfully"});
        }
    }
}
