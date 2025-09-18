using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace TMS.Authentication.Authenticate
{
    public class AuthenticationUser: IAuthenticationUser
    {
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        { 
             var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: Convert.ToString(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidIssuer"]),
                audience: Convert.ToString(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidAudience"]),
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:JwtExpireMint"])),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                ValidateAudience = true,
                ValidAudience = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidAudience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:Secret"])),
                ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
           
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        public  string ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:Secret"]));
            try
            {
                var validationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidAudience"],
                    ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
                };
                tokenHandler.ValidateToken(token, validationParams, out SecurityToken validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null)
                    return null;

                // Safely attempt to retrieve claims (only if they're actually there)
                var jkuClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "jku")?.Value;
                var userNameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "kid")?.Value;

                // Optionally log or return both if needed
                return userNameClaim ?? "UnknownUser";
            }
            catch
            {
                return null;
            }
        }
    }
}
