namespace Content.Core.Events.Common;

public class DomainEvent : IEvent
{
    public bool IsPublished { get; set; } = false;
}