using MassTransit;
using MessageContracts;
using Metadata.Infrastructure.UOW;

namespace Metadata.Infrastructure.MessageConsumers;

public class FileVersionDeactivatedConsumer : IConsumer<FileVersionDeactivated>
{
    private readonly IUnitOfWork _unitOfWork;

    public FileVersionDeactivatedConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<FileVersionDeactivated> context)
    {
        var version = await _unitOfWork.FileVersionRepository.FindAsync(context.Message.ID);

        version!.IsActive = false;
        await _unitOfWork.CommitAsync();
    }
}