using System.ComponentModel.DataAnnotations.Schema;
using Content.Core.Events.Common;

namespace Content.Core.Entities.Common;

public class EntityWithEvents : IEntityWithEvents
{
    [NotMapped] public List<IEvent> Events { get; set; } = new();
}