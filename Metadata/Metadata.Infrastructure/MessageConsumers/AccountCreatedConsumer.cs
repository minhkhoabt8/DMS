using AutoMapper;
using MassTransit;
using MessageContracts;
using Metadata.Core.Entities;
using Metadata.Infrastructure.UOW;

namespace Metadata.Infrastructure.MessageConsumers;

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
            // Add account
            await _unitOfWork.AccountRepository.AddAsync(account);

            // Add root folder
            var rootFolder = Folder.CreateRoot(account.ID);

            await _unitOfWork.FolderRepository.AddAsync(rootFolder);

            await _unitOfWork.CommitAsync();
        }
    }
}