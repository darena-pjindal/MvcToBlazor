using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVCToBlazor
{
    public class MvcAuthenticationStateProvider: AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MvcAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            var state = isAuthenticated? new AuthenticationState(_httpContextAccessor.HttpContext.User) : new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            return Task.FromResult(state);
        }
    }
}
