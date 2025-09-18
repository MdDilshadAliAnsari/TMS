using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TMS.Authentication.Authenticate;

public class NewAuthUser : IAuthenticationUserNew
{
    private readonly IConfiguration _configuration;

    public NewAuthUser(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Must be public to fulfill the interface contract
    //public JwtSecurityToken CreateTokenNew(List<Claim> authClaims)
    public JwtSecurityToken CreateTokenNew(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        _ = int.TryParse(_configuration["JWT:JwtExpireHour"], out int tokenValidityInHours);

         
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(tokenValidityInHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                  
        );

        return token;





    }

    // Must be public and match the interface method name
    public string GenerateRefreshTokenNew()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    // Must be public and match the interface method name
    public ClaimsPrincipal? GetPrincipalFromExpiredTokenNew(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidIssuer"],
            ValidateAudience = true,
            ValidAudience = TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:ValidAudience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:Secret"])),
            ValidateLifetime = true
            // here we are saying that we don't care about the token's expiration date
            //ValidateAudience = false,
            //ValidateIssuer = false,
            //ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TMS.Authentication.Authenticate.ConfigurationManager.AppSetting["JWT:Secret"])),
            //ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
