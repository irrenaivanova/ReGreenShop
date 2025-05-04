using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;

namespace ReGreenShop.Application.Common.Behaviors;

public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger logger;
    private readonly ICurrentUser currentUserService;
    private readonly IIdentity identityService;

    public RequestLogger(
        ILogger<TRequest> logger,
        ICurrentUser currentUserService,
        IIdentity identityService)
    {
        this.logger = logger;
        this.currentUserService = currentUserService;
        this.identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = this.currentUserService.UserId;
        var userName = userId != null ? await this.identityService.GetUserName(userId) : string.Empty;

        this.logger.LogInformation(
            "ReGreenShop Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName,
            userId,
            userName ?? "Anonymous",
            request);
    }
}

