using System.ComponentModel.DataAnnotations.Schema;
using Metadata.Core.Events.Common;

namespace Metadata.Core.Entities.Common;

public interface IEntityWithEvents
{
    [NotMapped] public List<IEvent> Events { get; set; }
}