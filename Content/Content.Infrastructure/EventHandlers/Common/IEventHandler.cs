using Content.Core.Events.Common;

namespace Content.Infrastructure.EventHandlers.Common;

public interface IEventHandler
{
    Task HandleAsync(IEvent evt);
}

public interface IEventHandler<T> : IEventHandler where T : IEvent
{
}