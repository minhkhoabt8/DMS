namespace Auth.Core.Events.Common;

public class IntegrationEvent : IEvent
{
    public bool IsPublished { get; set; } = false;
}