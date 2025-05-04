using MediatR;
using Microsoft.Extensions.Logging;
using ReGreenShop.Application.Common.Identity;
using ReGreenShop.Application.Common.Interfaces;
using System.Diagnostics;

namespace ReGreenShop.Application.Common.Behaviors;
public class RequestPerformanceBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch timer;
    private readonly ILogger<TRequest> logger;
    private readonly ICurrentUser currentUserService;
    private readonly IIdentity identityService;

    public RequestPerformanceBehavior(
        ILogger<TRequest> logger,
        ICurrentUser currentUserService,
        IIdentity identityService)
    {
        this.timer = new Stopwatch();

        this.logger = logger;
        this.currentUserService = currentUserService;
        this.identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request,
                RequestHandlerDelegate<TResponse> next,
                CancellationToken cancellationToken)
    {
        this.timer.Start();

        var response = await next();

        this.timer.Stop();

        var elapsedMilliseconds = this.timer.ElapsedMilliseconds;

        if (elapsedMilliseconds <= 500)
        {
            return response;
        }

        var requestName = typeof(TRequest).Name;
        var userId = this.currentUserService.UserId;
        var userName = userId != null ? await this.identityService.GetUserName(userId) : string.Empty;

        this.logger.LogWarning(
            "ReGreenShop Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
            requestName,
            elapsedMilliseconds,
            userId,
            userName ?? "Anonymous",
            request);

        return response;
    }
}
