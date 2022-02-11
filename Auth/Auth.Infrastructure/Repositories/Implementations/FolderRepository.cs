using Auth.Core.Entities;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repositories.Implementations.Common;
using Auth.Infrastructure.Repositories.Interfaces;

namespace Auth.Infrastructure.Repositories.Implementations;

public class FolderRepository : GenericRepository<Folder, AuthContext>, IFolderRepository
{
    public FolderRepository(AuthContext context) : base(context)
    {
    }
}