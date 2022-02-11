using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace VOL.API.Authorization
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            // For testing
            return true;
        }
    }
}