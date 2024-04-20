using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenereateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
    }
}
