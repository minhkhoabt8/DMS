using Metadata.Core.Entities;
using Metadata.Infrastructure.Repositories.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata.Infrastructure.Repositories.Interfaces
{
    public interface ITagRepository:
        IFindAsync<Tag>,
        IDelete<Tag>,
        IUpdate<Tag>,
        IAddAsync<Tag>
    {
    }
}
