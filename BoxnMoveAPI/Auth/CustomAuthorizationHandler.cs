using BoxnMove.Database;
using Microsoft.AspNetCore.Authorization;

namespace BoxnMoveAPI.Auth
{
    public class CustomAuthorizationHandler : AuthorizationHandler<CustomAuthRequirement>
    {
        private BoxnMoveDBContext _DbContext;

        public CustomAuthorizationHandler(BoxnMoveDBContext dbContext)
        {
            _DbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == requirement.ClaimType && c.Value == requirement.ClaimValue))
            {
                // Additional database check
                var userId = context.User.FindFirst("UserID")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    // Example: Check if the user has a specific permission in the database
                    if (true)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }

            context.Fail();
        }
    }
}
