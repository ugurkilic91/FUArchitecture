using System;
using System.Security.Claims;
using IdentityManagement.Infrastructure.Persistence;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagement.Infrastructure.Services
{
	public class IdentityClaimsProfileService: IProfileService
	{
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
        private readonly UserManager<AppUser> _userManager;

        public IdentityClaimsProfileService(UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> calimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = calimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(k => context.RequestedClaimTypes.Contains(k.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.Name));
            claims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
            claims.Add(new Claim("userEmailAdress", user.Email));
            claims.Add(new Claim(ClaimTypes.Role, "user"));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = true;
        }
    }
}

