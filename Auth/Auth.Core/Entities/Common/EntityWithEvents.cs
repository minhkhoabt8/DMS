using System.ComponentModel.DataAnnotations.Schema;
using Auth.Core.Events.Common;

namespace Auth.Core.Entities.Common;

public class EntityWithEvents : IEntityWithEvents
{
    [NotMapped] public List<IEvent> Events { get; set; } = new();
}