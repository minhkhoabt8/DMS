using Metadata.Core.Entities;
using Metadata.Core.Entities.Common;
using Metadata.Core.Events.Common;
using Metadata.Core.Extensions;
using Metadata.Infrastructure.EventHandlers.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Metadata.Infrastructure.Data;

public class MetadataContext : DbContext
{
    private readonly IEventDispatcher _eventDispatcher;

    public MetadataContext(DbContextOptions options, IEventDispatcher eventDispatcher) : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Core.Entities.File> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<Tag> Tags { get; set; }

    private void UpdateLastModified()
    {
        foreach (var entry in ChangeTracker.Entries<ITrackLastModified>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.LastModified = DateTime.Now.SetKindUtc();
                    break;
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateLastModified();

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