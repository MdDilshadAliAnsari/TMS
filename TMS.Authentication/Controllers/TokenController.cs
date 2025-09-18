using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TMS.Authentication.Authenticate; 
using TMS.Authentication.Model;

namespace TMS.Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController :   ControllerBase
    {
        #region  Variable Constructor and Dependency injection in Constructor level
        private readonly TMSDbContext _context;
        private readonly IAuthenticationUserNew _tokenService;
        public TokenController(TMSDbContext context, IAuthenticationUserNew tokenService)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }
        #endregion
        #region Refresh current Token  
        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid user request!!!");

            string  accessToken         = tokenApiModel.AccessToken;
            string  refreshToken        = tokenApiModel.RefreshToken;
            var     principal           = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var     username            = principal.Identity.Name; //this is mapped to the Name claim by default
            var     user                = _context.USERS.SingleOrDefault(u => u.USERNAME == username);
            
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var userData = new
            {
                user,
                username
            };
 
            var response = new { userData, Token = newAccessToken, RefreshToken = newRefreshToken };
            return Ok(new { response });
            
        }
        #endregion
        #region Revoke current Token and logout process
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var claims = ((ClaimsIdentity)User.Identity).Claims.ToList();
            string[] username = claims[1].ToString().Split(':'); 
            var user = _context.USERS.SingleOrDefault(u => u.USERNAME == username[1].Trim());
            if (user == null) return BadRequest();
            return NoContent();
        }
        #endregion


    }
}
