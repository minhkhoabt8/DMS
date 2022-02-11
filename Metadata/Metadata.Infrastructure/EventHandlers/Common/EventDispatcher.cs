using Metadata.Core.Events.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Metadata.Infrastructure.EventHandlers.Common;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventDispatcher(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceProvider = serviceProvider;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task DispatchAsync(IEvent evt)
    {
        var handlerType = typeof(IEventHandler<>).MakeGenericType(evt.GetType());
        IEventHandler? handler;

        // Intergration event should use a seperate scope because it may be processed after the initial request ended
        if (evt is IntegrationEvent)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            handler = (IEventHandler?) scope.ServiceProvider.GetService(handlerType);
            // Handler is registered

            await handler!.HandleAsync(evt);
        }
        // Handle domain events using same request scope
        else
        {
            handler = (IEventHandler?) _serviceProvider.GetService(handlerType);

            await handler!.HandleAsync(evt);
        }
    }
}