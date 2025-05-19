using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Users.Queries.GetAllUnreadNotificationsQuery;
public class UnReadNotificationsModel : IMapFrom<Notification>
{
    public string Title { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}
