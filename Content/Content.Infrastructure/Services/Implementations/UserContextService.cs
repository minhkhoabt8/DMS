using System.Security.Claims;
using Content.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Content.Infrastructure.Services.Implementations
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid ID => IsAuthenticated
            ? new(_contextAccessor.HttpContext.User.Claims.First(cl => cl.Type == ClaimTypes.NameIdentifier).Value)
            : throw new InvalidOperationException("User is not authenticated");

        public bool IsAuthenticated => _contextAccessor.HttpContext.User.Identity?.IsAuthenticated ?? false;
    }
}