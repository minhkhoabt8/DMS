using AutoMapper;
using Metadata.Core.Entities;
using Metadata.Core.Exceptions;
using Metadata.Infrastructure.DTOs.Tag;
using Metadata.Infrastructure.Services.Interfaces;
using Metadata.Infrastructure.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Services.Implementations
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;

        public TagService(IMapper mapper, IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
        }

        public async Task<TagReadDTO> CreateAsync(TagWriteDTO dto)
        {
            //check tag name exist
            if( await _unitOfWork.TagRepository.FindAsync(dto.Name) != null)
            {
                throw new UniqueConstraintException<Tag>(nameof(Tag.Name), dto.Name);
            }
            else
            {
                var tag = new Tag
                {
                    Name = dto.Name,
                };
               
                await _unitOfWork.TagRepository.AddAsync(tag);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<TagReadDTO>(tag);
            }
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagReadDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TagReadDTO> UpdateAsync(int id, TagWriteDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
