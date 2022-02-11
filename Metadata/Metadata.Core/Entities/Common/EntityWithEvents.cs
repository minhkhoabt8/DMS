using System.ComponentModel.DataAnnotations.Schema;
using Metadata.Core.Events.Common;

namespace Metadata.Core.Entities.Common;

public class EntityWithEvents : IEntityWithEvents
{
    [NotMapped] public List<IEvent> Events { get; set; } = new();
}