using MediatR;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Orders.Commands.FinishAnOrder;
public record FinishAnOrderCommand(FinishAnOrderModel model) : IRequest<Unit>
{
    public class FinishAnOrderCommandHandler : IRequestHandler<FinishAnOrderCommand, Unit>
    {
        private readonly IData data;
        private readonly IIdentity userService;
        private readonly INotifier notificationService;

        public FinishAnOrderCommandHandler(IData data, IIdentity userService, INotifier notificationService)
        {
            this.data = data;
            this.userService = userService;
            this.notificationService = notificationService;
        }

        public async Task<Unit> Handle(FinishAnOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await this.data.Orders.Include(x => x.Payment)
                .FirstOrDefaultAsync(x => x.Id == request.model.OrderId);
            if (order == null)
            {
                throw new NotFoundException("Order",request.model.OrderId);
            }
            var userId = order.UserId;
            var greenPointsReceived = 0;
            foreach (var item in request.model.GreenModels.Distinct())
            {
                var greenAlternative = await this.data.GreenAlternatives.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (greenAlternative == null)
                {
                    throw new NotFoundException("Green alternative", item.Id);
                }
                if (item.Quantity > greenAlternative.MaximumQuantity)
                {
                    throw new BusinessRulesException($"Maximum quantity of {greenAlternative.Name} is {greenAlternative.MaximumQuantity}");
                }
                order.OrderGreenAlternativeDetails.Add(new OrderGreenAlternativeDetail()
                {
                    GreenAlternative = greenAlternative,
                    Quantity = item.Quantity,
                });

                greenPointsReceived += item.Quantity * greenAlternative.RewardPoints;
            }

            if(greenPointsReceived > 0)
            {
                await this.userService.UpdateGreenPoints(userId, greenPointsReceived);
            }

            order.Status = Domain.Entities.Enum.OrderStatus.Delivered;
            order.Payment.Status = Domain.Entities.Enum.PaymentStatus.Completed;
            await this.data.SaveChangesAsync();

            var title = $"Order {order.Id} completed";
            var message = $"Order {order.Id} completed successfully. You have received {greenPointsReceived} Green Points.";
            await this.notificationService.NotifyUserAsync(userId, title, message);

            return Unit.Value;
        }
    }
}
