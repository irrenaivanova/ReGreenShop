using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Users.Commands;
public record ReadNotificationsCommand : IRequest<Unit>
{
    public class ReadNotificationsCommandHandler : IRequestHandler<ReadNotificationsCommand, Unit>
    {
        private readonly ICurrentUser currentUser;
        private readonly IData data;

        public ReadNotificationsCommandHandler(ICurrentUser currentUser, IData data)
        {
            this.currentUser = currentUser;
            this.data = data;
        }

        public async Task<Unit> Handle(ReadNotificationsCommand request, CancellationToken cancellationToken)
        {
            var userId = this.currentUser.UserId;
            if (userId == null)
            {
                throw new NotFoundException("User");
            }
            var unReadNotifications = await this.data.Notifications
                .Where(x => x.UserId == userId && !x.IsRead).ToListAsync();

            foreach (var notif in unReadNotifications)
            {
                notif.IsRead = true;
            }
            await this.data.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
