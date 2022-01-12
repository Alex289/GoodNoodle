using GoodNoodle.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace GoodNoodle.Domain;

public class ApplicationUser : IUser
{
    private readonly IHttpContextAccessor _accessor;

    public ApplicationUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    private string GetClaimValue(string claimType)
    {
        return _accessor.HttpContext?.User.Claims.Where(x => x.Type == claimType).FirstOrDefault().Value;
    }

    public Guid Id
    {
        get
        {
            var claim = GetClaimValue(ClaimTypes.NameIdentifier);

            if (claim == null || !Guid.TryParse(claim, out var id))
            {
                throw new InvalidOperationException(
                    "User id claim is missing or could not be parsed to a GUID");
            }

            return id;
        }
    }

    public string Role
    {
        get
        {
            var claim = GetClaimValue(ClaimTypes.Role);

            if (claim == null)
            {
                throw new InvalidOperationException(
                    "User role claim is missing");
            }

            return claim;
        }
    }

    public string Status
    {
        get
        {
            var claim = GetClaimValue(ClaimTypes.AuthorizationDecision);

            if (claim == null)
            {
                throw new InvalidOperationException("User status claim is missing");
            }

            return claim;
        }
    }
}
