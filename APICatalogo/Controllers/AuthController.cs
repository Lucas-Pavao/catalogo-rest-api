using APICatalogo.DTOs;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            if(user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name, model.UserName!),
                new Claim(ClaimTypes.Email, model.UserEmail!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenereateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(
                    new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo,
                    }
                    );
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("resgister")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model) {

            var userExists = await _userManager.FindByNameAsync(model.UserName!);
            if (userExists != null) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User already exists!" });
            }
            ApplicationUser user = new() {
            Email = model.Email,
            UserName = model.UserName,
            SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "User creation failed!" });
            }
            return Ok(new Response { Status = "Sucess", Message = "User create Sucessfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> refreshToken(TokenModel model) { 
            if(model == null)
            {
                return BadRequest("Invalid Client Request");
            }
            string? acessToken = model.AccessToken ?? throw new ArgumentNullException(nameof(model.AccessToken));
            string? refreshToken = model.RefreshToken ?? throw new ArgumentNullException(nameof(model.AccessToken));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken!, _configuration);
            
            if (principal == null)
            {
                return BadRequest("Invalid access token / refresh token! ");
            }


            string username = principal.Identity!.Name!;
            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) {

                return BadRequest("Invalid access token / refresh token! ");
            }


            var newAccessToken = _tokenService.GenereateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                acessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });

        }
            
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();


        }


    }

    

    
}
