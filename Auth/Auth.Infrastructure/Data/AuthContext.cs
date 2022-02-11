using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Core.Entities.Common;
using Auth.Core.Events.Common;
using Auth.Infrastructure.EventHandlers.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using File = Auth.Core.Entities.File;

namespace Auth.Infrastructure.Data;

public class AuthContext : DbContext
{
    private readonly IEventDispatcher _eventDispatcher;

    public AuthContext(DbContextOptions options, IEventDispatcher eventDispatcher) : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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