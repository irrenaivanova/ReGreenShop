using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Infrastructure.Services;
public class NotifierService : INotifier
{
    private readonly IData data;
    private readonly ILogger logger;
    private readonly UserManager<User> userManager;

    public NotifierService(IData data,
                ILogger<NotifierService> logger,
                UserManager<User> userManager)
    {
        this.data = data;
        this.logger = logger;
        this.userManager = userManager;
    }

    public async Task NotifyAdminAsync(string title, string message)
    {
        var admin = await this.userManager.FindByEmailAsync(AdminEmail);
        var notification = new Notification()
        {
            UserId = admin!.Id,
            Text = message,
            Title = title,
        };
        this.data.Notifications.Add(notification);
        await this.data.SaveChangesAsync();

        this.logger.LogInformation("Admin alert: {@Title} - {@Message}", title, message);
    }

    public async Task NotifyUserAsync(string userId, string title, string message)
    {
        var notification = new Notification()
        {
            UserId = userId,
            Text = message,
            Title = title,
        };
        this.data.Notifications.Add(notification);
        await this.data.SaveChangesAsync();
    }
}
