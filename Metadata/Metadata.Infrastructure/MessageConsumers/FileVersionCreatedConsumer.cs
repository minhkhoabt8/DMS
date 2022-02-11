using AutoMapper;
using MassTransit;
using MessageContracts;
using Metadata.Core.Entities;
using Metadata.Infrastructure.UOW;

namespace Metadata.Infrastructure.MessageConsumers;

public class FileVersionCreatedConsumer : IConsumer<FileVersionCreated>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FileVersionCreatedConsumer(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<FileVersionCreated> context)
    {
        var version = _mapper.Map<FileVersion>(context.Message);

        if ((await _unitOfWork.FileVersionRepository.FindAsync(version.ID)) == null)
        {
            await _unitOfWork.FileVersionRepository.AddAsync(version);
            await _unitOfWork.CommitAsync();
        }
    }
}