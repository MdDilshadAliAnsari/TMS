using System.Security.Claims;

namespace TMS.Authentication.Authenticate
{
    public interface IAuthenticationUser 
    { 
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string ValidateToken(string token);

    
    }
}
