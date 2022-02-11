using Metadata.Core.Entities;
using Metadata.Infrastructure.Data;
using Metadata.Infrastructure.Repositories.Implementations.Common;
using Metadata.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Implementations
{
    public class TagRepository : GenericRepository<Tag, MetadataContext>, ITagRepository
    {
        public TagRepository(MetadataContext context) : base(context)
        {
        }
       
    }
    


}
