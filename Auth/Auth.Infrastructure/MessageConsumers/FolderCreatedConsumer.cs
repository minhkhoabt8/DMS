using System.Threading.Tasks;
using Auth.Core.Entities;
using Auth.Infrastructure.UOW;
using AutoMapper;
using MassTransit;
using MessageContracts;

namespace Auth.Infrastructure.MessageConsumers;

public class FolderCreatedConsumer : IConsumer<FolderCreated>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FolderCreatedConsumer(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<FolderCreated> context)
    {
        var folder = _mapper.Map<Folder>(context.Message);

        // File not created yet
        if ((await _unitOfWork.FolderRepository.FindAsync(folder.ID)) == null)
        {
            await _unitOfWork.FolderRepository.AddAsync(folder);
            await _unitOfWork.CommitAsync();
        }
    }
}