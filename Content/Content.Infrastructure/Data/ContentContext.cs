using Content.Core.Entities;
using Content.Core.Entities.Common;
using Content.Core.Events.Common;
using Content.Infrastructure.EventHandlers.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.Data;

public class ContentContext : DbContext
{
    private readonly IEventDispatcher _eventDispatcher;

    public ContentContext(DbContextOptions options, IEventDispatcher eventDispatcher) : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<FileVersion> FileVersions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await DispatchEvents<DomainEvent>();
        var result = await base.SaveChangesAsync(cancellationToken);
        _ = DispatchEvents<IntegrationEvent>();

        return result;
    }

    private async Task DispatchEvents<T>()
    {
        while (true)
        {
            var events = ChangeTracker
                .Entries<IEntityWithEvents>()
                .SelectMany(x => x.Entity.Events)
                .Where(domainEvent => !domainEvent.IsPublished && domainEvent is T).ToList();

            if (!events.Any())
            {
                break;
            }

            foreach (var eve in events)
            {
                Log.Information($"Dispatching event of type {eve.GetType().Name}");
                try
                {
                    await _eventDispatcher.DispatchAsync(eve);
                    eve.IsPublished = true;
                }
                catch (Exception e)
                {
                    Log.Error($"Error dispatching event {events} due to error: {e.Message}");
                    throw;
                }
            }
        }
    }
}