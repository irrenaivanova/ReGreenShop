using Microsoft.EntityFrameworkCore;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Common.Interfaces;

public interface IData
{
    DbSet<Address> Addresses { get; set; }

    DbSet<Cart> Carts { get; set; }

    DbSet<CartItem> CartItems { get; set; }

    DbSet<Category> Categories { get; set; }

    DbSet<City> Cities { get; set; }

    DbSet<ContactForm> ContactForms { get; set; }

    DbSet<DeliveryPrice> DeliveryPrices { get; set; }

    DbSet<GreenAlternative> GreenAlternatives { get; set; }

    DbSet<Image> Images { get; set; }

    DbSet<Label> Labels { get; set; }

    DbSet<LabelProduct> LabelProducts { get; set; }

    DbSet<Notification> Notifications { get; set; }

    DbSet<Order> Orders { get; set; }

    DbSet<OrderDetail> OrderDetails { get; set; }

    DbSet<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }

    DbSet<Payment> Payments { get; set; }

    DbSet<PaymentMethod> PaymentMethods { get; set; }

    DbSet<DiscountVoucher> DiscountVouchers { get; set; }

    DbSet<Product> Products { get; set; }

    DbSet<ProductCategory> ProductCategories { get; set; }

    DbSet<UserLikeProduct> UserLikeProducts { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default!);
}
