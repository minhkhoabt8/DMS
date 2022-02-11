using Content.Core.Events.Common;

namespace Content.Infrastructure.EventHandlers.Common;

public interface IEventDispatcher
{
    Task DispatchAsync(IEvent domainEvent);
}