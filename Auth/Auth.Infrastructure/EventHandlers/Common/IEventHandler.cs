using System.Threading.Tasks;
using Auth.Core.Events.Common;

namespace Auth.Infrastructure.EventHandlers.Common;

public interface IEventHandler
{
    Task HandleAsync(IEvent evt);
}

public interface IEventHandler<T> : IEventHandler where T : IEvent
{
}