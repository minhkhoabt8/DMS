using Metadata.Core.Events.Common;

namespace Metadata.Infrastructure.EventHandlers.Common;

public interface IEventHandler
{
    Task HandleAsync(IEvent evt);
}

public interface IEventHandler<T> : IEventHandler where T : IEvent
{
}