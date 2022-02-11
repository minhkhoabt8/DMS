﻿using Content.Infrastructure.Repositories.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace Content.Infrastructure.Repositories.Implementations.Common;

public class GenericRepository<TEntity, TContext> :
    IGetAllAsync<TEntity>,
    IFindAsync<TEntity>,
    IAddAsync<TEntity>,
    IUpdate<TEntity>,
    IDelete<TEntity> where TEntity : class where TContext : DbContext
{
    protected readonly TContext _context;

    public GenericRepository(TContext context)
    {
        _context = context;
    }

    public virtual async Task AddAsync(TEntity obj)
    {
        await _context.Set<TEntity>().AddAsync(obj);
    }

    public virtual void Delete(TEntity obj)
    {
        _context.Set<TEntity>().Remove(obj);
    }

    public virtual async Task<TEntity?> FindAsync(params object[] keys)
    {
        return await _context.Set<TEntity>().FindAsync(keys);
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync(bool trackChanges = false)
    {
        IQueryable<TEntity> dbSet = _context.Set<TEntity>();
        if (trackChanges == false)
        {
            dbSet = dbSet.AsNoTracking();
        }

        return Task.FromResult(dbSet.AsEnumerable());
    }

    public virtual void Update(TEntity obj)
    {
        _context.Set<TEntity>().Update(obj);
    }
}