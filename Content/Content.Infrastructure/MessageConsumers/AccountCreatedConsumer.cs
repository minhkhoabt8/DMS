using AutoMapper;
using Content.Core.Entities;
using Content.Infrastructure.UOW;
using MassTransit;
using MessageContracts;

namespace Content.Infrastructure.MessageConsumers;

public class AccountCreatedConsumer : IConsumer<AccountCreated>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AccountCreatedConsumer(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<AccountCreated> context)
    {
        var account = _mapper.Map<Account>(context.Message);

        // Account not yet created
        if ((await _unitOfWork.AccountRepository.FindAsync(account.ID)) == null)
        {
            await _unitOfWork.AccountRepository.AddAsync(account);
            await _unitOfWork.CommitAsync();
        }
    }
}