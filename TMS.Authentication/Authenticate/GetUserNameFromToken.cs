using System.Security.Claims;

namespace TMS.Authentication.Authenticate
{
    public class GetUserNameFromToken
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserNameFromToken(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
