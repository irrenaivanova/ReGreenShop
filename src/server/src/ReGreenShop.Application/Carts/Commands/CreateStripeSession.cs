using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Interfaces;
using Stripe.Checkout;

namespace ReGreenShop.Application.Carts.Commands;
public record CreateStripeSession([FromBody] string orderId) : IRequest<string>
{
    public class CreateStripeSessionHandler : IRequestHandler<CreateStripeSession, string>
    {
        private readonly IData data;

        public CreateStripeSessionHandler(IData data)
        {
            this.data = data;
        }

        public async Task<string> Handle(CreateStripeSession request, CancellationToken cancellationToken)
        {
            var order = await data.Orders.FirstOrDefaultAsync(x => x.Id == request.orderId);
            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)order.TotalPrice*100,
                        Currency = "bgn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "ReGreenShop Cart",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "https://localhost:5173/",
                CancelUrl = "https://localhost:5173/",
            };

            try
            {
                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                return session.Id;
            }
            catch (Exception e)
            {
                throw new BusinessRulesException("Stripe");
            }
        }
    }
}
