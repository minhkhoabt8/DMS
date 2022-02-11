using AutoMapper;
using Content.Infrastructure.UOW;
using MassTransit;
using MessageContracts;
using File = Content.Core.Entities.File;

namespace Content.Infrastructure.MessageConsumers;

public class FileCreatedConsumer : IConsumer<FileCreated>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FileCreatedConsumer(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FileCreated> context)
    {
        var file = _mapper.Map<File>(context.Message);

        // File not created yet
        if ((await _unitOfWork.FileRepository.FindAsync(file.ID)) == null)
        {
            await _unitOfWork.FileRepository.AddAsync(file);
            await _unitOfWork.CommitAsync();
        }
    }
}