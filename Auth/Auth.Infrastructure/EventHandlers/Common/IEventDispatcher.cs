using System.Threading.Tasks;
using Auth.Core.Events.Common;

namespace Auth.Infrastructure.EventHandlers.Common;

public interface IEventDispatcher
{
    Task DispatchAsync(IEvent domainEvent);
}