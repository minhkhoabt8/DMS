using System;
using System.Threading.Tasks;
using Auth.Core.Events.Common;
using Auth.Core.Events.Integration;
using Auth.Infrastructure.EventHandlers.Common;
using AutoMapper;
using MassTransit;
using MessageContracts;

namespace Auth.Infrastructure.EventHandlers.Integration;

public class AccountPostCreatedEventHandler : IEventHandler<AccountPostCreatedEvent>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEnpoint;

    public AccountPostCreatedEventHandler(IPublishEndpoint publishEnpoint, IMapper mapper)
    {
        _publishEnpoint = publishEnpoint;
        _mapper = mapper;
    }

    public async Task HandleAsync(IEvent evt)
    {
        var account = ((AccountPostCreatedEvent) evt).Account;
        try
        {
            await _publishEnpoint.Publish(_mapper.Map<AccountCreated>(account));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}