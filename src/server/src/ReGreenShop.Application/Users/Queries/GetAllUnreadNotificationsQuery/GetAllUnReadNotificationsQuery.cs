using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;

namespace ReGreenShop.Application.Users.Queries.GetAllUnreadNotificationsQuery;
public record GetAllUnReadNotificationsQuery : IRequest<IEnumerable<UnReadNotificationsModel>>
{
    public class GetAllUnReadNotificationsQueryHandler : IRequestHandler<GetAllUnReadNotificationsQuery, IEnumerable<UnReadNotificationsModel>>
    {
        private readonly ICurrentUser currentUser;
        private readonly IData data;

        public GetAllUnReadNotificationsQueryHandler(ICurrentUser currentUser, IData data)
        {
            this.currentUser = currentUser;
            this.data = data;
        }

        public async Task<IEnumerable<UnReadNotificationsModel>> Handle(GetAllUnReadNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = this.currentUser.UserId;
            if (userId == null)
            {
                throw new NotFoundException("User");
            }
            var unReadNotifications = await this.data.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .To<UnReadNotificationsModel>()
                .ToListAsync();
            return unReadNotifications;
        }
    }
}
