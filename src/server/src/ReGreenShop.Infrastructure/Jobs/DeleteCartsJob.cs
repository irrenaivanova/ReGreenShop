using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Infrastructure.Jobs;
public class DeleteCartsJob
{
    private readonly IDeleteUnusedCart deleteCartService;

    public DeleteCartsJob(IDeleteUnusedCart deleteCartService)
    {
        this.deleteCartService = deleteCartService;
    }
    public Task Execute(CancellationToken cancellationToken)
    {
        return this.deleteCartService.DeleteUnusedCartsDaily();
    }
}
