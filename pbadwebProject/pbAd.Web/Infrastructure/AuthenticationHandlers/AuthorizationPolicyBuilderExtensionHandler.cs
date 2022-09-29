using Microsoft.AspNetCore.Authorization;

namespace pbAd.Web.Infrastructure.AuthenticationHandlers
{
    public static class AuthorizationPolicyBuilderExtensionHandler
    {
        public static AuthorizationPolicyBuilder UserRequireCustomClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new CustomUserRequireClaim(claimType));
            return builder;
        }
    }
}
