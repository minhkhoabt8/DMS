using Metadata.Core.Events.Common;

namespace Metadata.Infrastructure.EventHandlers.Common;

public interface IEventDispatcher
{
    Task DispatchAsync(IEvent domainEvent);
}