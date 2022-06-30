using Microsoft.AspNetCore.Authorization;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT
{
    public class AuthorizationPolicies
    {
        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("Admin").Build();
        }
        public static AuthorizationPolicy CustomerPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("Customer").Build();
        }
        public static AuthorizationPolicy DelivererPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("Deliverer").Build();
        }
    }
}
