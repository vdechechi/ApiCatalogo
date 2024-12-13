using ApiCatalogo.DTO;
using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiCatalogo.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginModel model) 
        {
            var user = await _userManager.FindByNameAsync(model.Username!);

            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                    out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]

        public async Task<IActionResult> register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);

            if(userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User already exists!"});
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                   new Response { Status = "Error", Message = "User creation Failed" });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("refresh-token")]

        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if(tokenModel is null)
            {
                return BadRequest("invalid client request");
            }
            string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof (tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal is null)
            {
                return BadRequest("invalid client token/refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("invalid client token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return Ok(new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
            })
                );
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
