using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Jobs;
public class PromoJob
{
    private readonly IPromo promoService;

    public PromoJob(IPromo promoService)
    {
        this.promoService = promoService;
    }

    public Task Execute(CancellationToken cancellationToken)
    {
        return this.promoService.RefreshWeeklyPromosAsync();
    }
}
