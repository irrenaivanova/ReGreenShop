using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReGreenShop.Application.Common.Interfaces;
using ReGreenShop.Domain.common;
using ReGreenShop.Domain.Entities;
using ReGreenShop.Infrastructure.Persistence.Identity;
using Label = ReGreenShop.Domain.Entities.Label;

namespace ReGreenShop.Infrastructure.Persistence;
public class ApplicationDbContext : IdentityDbContext<User, Role, string>, IData
{
    private readonly IDateTime dateTime;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDateTime dateTime)
        : base(options)
    {
        this.dateTime = dateTime ?? throw new ArgumentNullException(nameof(dateTime));
    }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<City> Cities { get; set; }

    public DbSet<ContactForm> ContactForms { get; set; }

    public DbSet<DeliveryPrice> DeliveryPrices { get; set; }

    public DbSet<GreenAlternative> GreenAlternatives { get; set; }

    public DbSet<Image> Images { get; set; }

    public DbSet<Label> Labels { get; set; }

    public DbSet<LabelProduct> LabelProducts { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    public DbSet<OrderGreenAlternativeDetail> OrderGreenAlternativeDetails { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<DiscountVoucher> DiscountVouchers { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<UserLikeProduct> UserLikeProducts { get; set; }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = this.dateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedOn = this.dateTime.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Needed for Identity models configuration
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        ConfigureDeletableEntities(builder);

        SetGlobalQueryFilters(builder);

        DisableCascadeDeletes(builder);

        builder.Entity<IdentityUserClaim<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(uc => uc.UserId);

        builder.Entity<IdentityUserLogin<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ul => ul.UserId);

        builder.Entity<IdentityUserRole<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId);

    }

    // Set index on IsDeleted for entities that implement IDeletableEntity
    private void ConfigureDeletableEntities(ModelBuilder builder)
    {
        var deletableEntityTypes = builder.Model
            .GetEntityTypes()
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));

        foreach (var deletableEntityType in deletableEntityTypes)
        {
            builder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
        }
    }

    // Disable cascade delete for all foreign keys
    private void DisableCascadeDeletes(ModelBuilder builder)
    {
        var entityTypes = builder.Model.GetEntityTypes().ToList();
        var foreignKeys = entityTypes
            .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

        foreach (var foreignKey in foreignKeys)
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    // Set global query filter for entities implementing IDeletableEntity
    private void SetGlobalQueryFilters(ModelBuilder builder)
    {
        var entityTypes = builder.Model.GetEntityTypes();

        var deletableEntityTypes = entityTypes
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));

        foreach (var deletableEntityType in deletableEntityTypes)
        {
            var method = typeof(ApplicationDbContext)
                .GetMethod(nameof(SetIsDeletedQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(deletableEntityType.ClrType);

            method.Invoke(null, new object[] { builder });
        }
    }

    private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
    where T : class, IDeletableEntity
    {
        builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }
}
