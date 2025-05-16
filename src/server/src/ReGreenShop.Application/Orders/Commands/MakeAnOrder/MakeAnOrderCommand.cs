
using System.IO;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Carts.Queries.ViewProductsInCartQuery.Models;
using ReGreenShop.Application.Common.Exceptions;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Application.Common.Mappings;
using ReGreenShop.Application.Common.Models;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Domain.Services;
using static ReGreenShop.Application.Common.GlobalConstants;

namespace ReGreenShop.Application.Orders.Commands.MakeAnOrder;
public record MakeAnOrderCommand(MakeAnOrderModel model) : IRequest<string>
{
    public class MakeAnOrderCommandHandler : IRequestHandler<MakeAnOrderCommand, string>
    {
        private readonly ICurrentUser currentUser;
        private readonly IData data;
        private readonly ICart cartService;
        private readonly IDelivery deliveryService;
        private readonly IIdentity userService;
        private readonly IPdfGenerator pdfGenerator;
        private readonly IStorage storage;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment web;

        public MakeAnOrderCommandHandler(ICurrentUser currentUser, IData data, ICart cartService, IDelivery deliveryService,
                                        IIdentity userService, IPdfGenerator pdfGenerator,IStorage storage, IEmailSender emailSender,
                                        IWebHostEnvironment web)
        {
            this.currentUser = currentUser;
            this.data = data;
            this.cartService = cartService;
            this.deliveryService = deliveryService;
            this.userService = userService;
            this.pdfGenerator = pdfGenerator;
            this.storage = storage;
            this.emailSender = emailSender;
            this.web = web;
        }

        public async Task<string> Handle(MakeAnOrderCommand request, CancellationToken cancellationToken)
        {
            // check if the user is authenticated
            var userId =  this.currentUser.UserId;
            if (userId == null)
            {
                throw new AuthenticationException("Please log in to complete your purchase.");
            }

            var cartId = await this.cartService.GetCartIdAsync();
            var cartItems = await this.data.CartItems.Where(x => x.CartId == cartId)
                .Select(x => x.Product)
                .To<ProductInCartModel>()
                .ToListAsync();

            foreach (var item in cartItems)
            {
                var quantity = this.data.CartItems.First(x => x.ProductId == item.Id && x.CartId == cartId).Quantity;
                item.QuantityInCart = quantity;
            }

            // check the stock of the products
            foreach (var item in cartItems)
            {
                var product = await this.data.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == item.Id);
                if (product == null)
                {
                    throw new NotFoundException("Product", "null");
                }
                var stock = product.Stock;
                if (item.QuantityInCart > stock)
                {
                    throw new InsufficientQuantityException(item.Name, stock);
                }
            }

            // Calculate the price including promotions
            foreach (var item in cartItems)
            {
                if (item.HasTwoForOneDiscount)
                {
                    item.TotalPriceProduct = PriceCalculator.CalculateTwoForOnePrice(item.Price, item.QuantityInCart);
                    item.LabelTwoForOne = "TwoForOne";
                }
                else if (item.HasPromoDiscount && !item.HasTwoForOneDiscount && item.DiscountPercentage.HasValue)
                {
                    item.DiscountPrice = PriceCalculator.CalculateDiscountedPrice(item.Price, item.DiscountPercentage.Value);
                    item.TotalPriceProduct = PriceCalculator.CalculateTotalPrice(item.DiscountPrice.Value, item.QuantityInCart);
                }
                else
                {
                    item.TotalPriceProduct = PriceCalculator.CalculateTotalPrice(item.Price, item.QuantityInCart);
                }
            }

            // TODO : Check if any product prices have changed and notify the user
            var totalPriceProducts = cartItems.Sum(x => x.TotalPriceProduct);

            // Calculate the delivery price
            (decimal? deliveryCost, string deliveryMessage) = this.deliveryService.CalculateTheDeliveryPrice(totalPriceProducts);

            if (deliveryCost == null)
            {
                throw new BusinessRulesException(deliveryMessage);
            }

            // Apply discount to total price if a discount voucher is used
            int? voucherId = request.model.DiscountVoucherId;
            var discount = 0m;
            int greenPoints = 0;

            if (voucherId != null)
            {
                var voucher = await this.data.DiscountVouchers.FirstOrDefaultAsync(x => x.Id == voucherId);
                if (voucher == null)
                {
                    throw new NotFoundException("Voucher");
                }
                discount = voucher.PriceDiscount;
                greenPoints = voucher.GreenPoints;

                var userDto = await this.userService.GetUserWithAdditionalInfo();
                var userPoints = userDto.TotalGreenPoints;
                if (!userPoints.HasValue || greenPoints > userPoints!.Value)
                {
                    throw new BusinessRulesException("Insufficient green points to apply this voucher.");
                }
            }

            // Calculate the total price including delivery price and discounts
            var totalPriceOrder = Math.Round(totalPriceProducts + deliveryCost.Value - discount,2);

            // Create a new delivery address if one does not already exist
            var address = await this.data.Addresses.Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.CityId == request.model.CityId && x.Street == request.model.Street && x.Number == request.model.Number);

            if (address == null)
            {
                var city = await this.data.Cities.FirstOrDefaultAsync(x => x.Id == request.model.CityId);
                if (city == null)
                {
                    throw new NotFoundException("City", request.model.CityId);
                }

                address = new Address()
                {
                    CityId = request.model.CityId,
                    Street = request.model.Street,
                    Number = request.model.Number,
                    UserId = userId,
                };

                this.data.Addresses.Add(address);
            }

            // TODO : Integrate Stripe for payment processing and status tracking
            var newPayment = new Payment()
            {
                PaymentMethodId = request.model.PaymentMethodId,
                Status = Domain.Entities.Enum.PaymentStatus.Pending,
            };

            // Create new order
            var newOrder = new Order()
            {
                UserId = userId,
                DeliveryDate = request.model.DeliveryDateTime,
                TotalPrice = totalPriceOrder,
                Status = Domain.Entities.Enum.OrderStatus.Pending,
                Address = address,
                Payment = newPayment,
                DiscountVoucherId = request.model.DiscountVoucherId,    
            };

            // Add order details
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = item.Id,
                    Order = newOrder,
                    Quantity = item.QuantityInCart,
                    PricePerUnit = item.DiscountPrice ?? item.Price,
                };

                this.data.OrderDetails.Add(orderDetail);
            }

            // Reduce the quantity available in stock
            foreach (var item in cartItems)
            {
                var product = await this.data.Products.FirstOrDefaultAsync(x => x.Id == item.Id);

                if (product == null)
                {
                    throw new NotFoundException("Product");
                }

                if (product.Stock < item.QuantityInCart)
                {
                    throw new InsufficientQuantityException(product.Name, item.QuantityInCart);
                }
                product!.Stock -= item.QuantityInCart;
            }

            // Clear the user cart
            await this.cartService.ClearCartAsync();
            await this.data.SaveChangesAsync();


            // Update user info and decrease ReGreenPoints if a voucher is applied
            var changeUserDto = new ChangeUserModel()
            {
                AddressId = address.Id,
                FirstName = request.model.FirstName,
                LastName = request.model.LastName,
                GreenPoints = greenPoints
            };

            await this.userService.ChangeUserInfoAsync(changeUserDto);

            var invoiceItem = new InvoiceItem()
            {
                TotalPrice = totalPriceOrder,
                TotalPriceProducts = totalPriceProducts,
                DeliveryPrice = deliveryCost.Value,
                Discount = discount,
                Products = cartItems.Select(x => new ProductForInvoice()
                {
                    Name = x.HasTwoForOneDiscount ? x.Name + "   Promo: 2 For 1" : x.Name,
                    Quantity = x.QuantityInCart,
                    PricePerUnit = x.DiscountPrice ?? x.Price,
                    TotalPrice = x.TotalPriceProduct,

                }).ToList()
            };

            // Create invoice
            var invoice = this.pdfGenerator.GenerateReceiptPdfAsync(invoiceItem);
            var invoiceUrl = await this.storage.SaveInvoicesAsync(invoice, $"invoice {newOrder.Id}");
            newOrder.InvoiceUrl = invoiceUrl;
            await this.data.SaveChangesAsync();

            // Send an email with the Poly policy attached

            string templateId = "d-5e226b6ae4c4434cadfee761ff05ea51";
            var dynamicDta = new Dictionary<string, object>
                    {
                        { "name1", $"{request.model.FirstName} {request.model.LastName}" },
                        { "name2", $"{newOrder.Id.Substring(0,6)} / {newOrder.CreatedOn.ToString("dd MMM yyyy")}" },
                    };

            var path = Path.Combine(this.web.WebRootPath, "invoices", $"invoice {newOrder.Id}.pdf");
            var fileBytes = await System.IO.File.ReadAllBytesAsync(path);
            var attachment = new EmailAttachment
            {
                FileName = "invoice.pdf",
                Content = fileBytes,
                MimeType = "application/pdf"
            };

            var userName = await this.userService.GetUserName(userId);
            await this.emailSender.SendTemplateEmailAsync(SystemEmailSender, SystemEmailSenderName,userName!,templateId, dynamicDta, new List<EmailAttachment> { attachment });
            return newOrder.Id;
        }
    }
}
