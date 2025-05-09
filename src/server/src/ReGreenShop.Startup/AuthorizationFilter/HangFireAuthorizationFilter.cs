using Hangfire.Dashboard;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Startup.AuthorizationFilter;
public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        if (httpContext != null && httpContext.User.Identity!.IsAuthenticated)
        {
            return httpContext.User.IsInRole(AdminName);
        }

        return false;
    }
}
