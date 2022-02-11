using MassTransit;
using MessageContracts;
using Metadata.Infrastructure.UOW;

namespace Metadata.Infrastructure.MessageConsumers;

public class FileVersionReadyConsumer : IConsumer<FileVersionReady>
{
    private readonly IUnitOfWork _unitOfWork;

    public FileVersionReadyConsumer(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<FileVersionReady> context)
    {
        var version = await _unitOfWork.FileVersionRepository.FindAsync(context.Message.ID);

        version!.IsReady = true;
        await _unitOfWork.CommitAsync();
    }
}