using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories.Implementations.Common;
using Auth.Infrastructure.Repositories.Interfaces;
using File = Auth.Core.Entities.File;

namespace Auth.Infrastructure.Repositories.Implementations;

public class FileRepository : GenericRepository<File, AuthContext>, IFileRepository
{
    public FileRepository(AuthContext context) : base(context)
    {
    }
}