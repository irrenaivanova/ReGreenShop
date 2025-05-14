using MediatR;

namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public record MakeAnOrderCommand(MakeAnOrderModel model) : IRequest<int>
{
    public class MakeAnOrderCommandHandler : IRequestHandler<MakeAnOrderCommand, int>
    {
        public Task<int> Handle(MakeAnOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
