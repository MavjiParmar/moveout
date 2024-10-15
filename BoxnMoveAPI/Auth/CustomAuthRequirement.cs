using Microsoft.AspNetCore.Authorization;

namespace BoxnMoveAPI.Auth
{
    public class CustomAuthRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public string ClaimValue { get; }
        public string Permission { get; }

        public CustomAuthRequirement(string claimType, string claimValue, string permission)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
            Permission = permission;
        }
    }
}
