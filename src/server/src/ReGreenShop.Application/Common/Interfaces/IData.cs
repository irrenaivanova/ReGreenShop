using Microsoft.EntityFrameworkCore;
using ReGreenShop.Domain.Entities;

namespace ReGreenShop.Application.Common.Interfaces;

public interface IData
{
    DbSet<Product> Products { get; set; }

    Task<int> SaveChanges(CancellationToken cancellationToken);
}
