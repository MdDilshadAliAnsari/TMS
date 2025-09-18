using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TMS.Authentication.Authenticate
{
    public interface IAuthenticationUserNew
    {
        #region New Auth Class
         JwtSecurityToken CreateTokenNew(List<Claim> authClaims);
  
        string GenerateRefreshTokenNew();
        ClaimsPrincipal? GetPrincipalFromExpiredTokenNew(string? token);
        #endregion
    }

}
