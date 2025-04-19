using Microsoft.EntityFrameworkCore;
using OA.GenericRepository.GenericRepositoryPattern.Interfaces;
using System.Linq.Expressions;

namespace OA.GenericRepository.GenericRepositoryPattern.Implementations;
public class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _context;

    public Repository(TContext context)
    {
        _context = context;
        Entity = _context.Set<TEntity>();
    }

    private DbSet<TEntity> Entity;

    public void Add(TEntity entity)
    {
        Entity.Add(entity);
    }
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entity.AddAsync(entity, cancellationToken);
    }
    public void AddRange(ICollection<TEntity> entities)
    {
        Entity.AddRange(entities);
    }
    public async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Entity.AddRangeAsync(entities, cancellationToken);
    }
    public IQueryable<TEntity> GetAll()
    {
        return GetQuery(false);
    }
    public IQueryable<TEntity> GetAllWithTracking()
    {
        return GetQuery(true); // Takip ederek getir
    }
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return GetQuery(false).Where(expression);
    }
    public IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression)
    {
        return GetQuery(true).Where(expression);
    }
    public TEntity First(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
    {
        return GetQuery(isTrackingActive).First(expression);
    }
    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true)
    {
        return await GetQuery(isTrackingActive).FirstAsync(expression, cancellationToken);
    }
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true)
    {
        return GetQuery(isTrackingActive).FirstOrDefault(expression);
    }
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true)
    {
        return await GetQuery(isTrackingActive).FirstOrDefaultAsync(expression, cancellationToken);
    }
    public TEntity GetByExpression(Expression<Func<TEntity, bool>> expression)
    {
        return GetQuery(false).FirstOrDefault(expression);
    }
    public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await GetQuery(false).FirstOrDefaultAsync(expression, cancellationToken);
    }
    public TEntity GetByExpressionWithTracking(Expression<Func<TEntity, bool>> expression)
    {
        return GetQuery(true).FirstOrDefault(expression);
    }
    public async Task<TEntity> GetByExpressionWithTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await GetQuery(true).FirstOrDefaultAsync(expression, cancellationToken);
    }
    public TEntity GetFirst()
    {
        return GetQuery(false).First();
    }
    public async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        return await GetQuery(false).FirstAsync(cancellationToken);
    }
    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        return GetQuery(false).Any(expression);
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await GetQuery(false).AnyAsync(expression, cancellationToken);
    }
    public IQueryable<KeyValuePair<bool, int>> CountBy(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return Entity
            .GroupBy(expression)
            .Select(g => new KeyValuePair<bool, int>(g.Key, g.Count()));
    }
    public void Update(TEntity entity)
    {
        Entity.Update(entity);
    }
    public void UpdateRange(ICollection<TEntity> entities)
    {
        Entity.UpdateRange(entities);
    }
    public void Delete(TEntity entity)
    {
        Entity.Remove(entity);
    }
    public void DeleteRange(ICollection<TEntity> entities)
    {
        Entity.RemoveRange(entities);
    }
    public async Task DeleteByGuidIdAsync(Guid id)
    {
        var entity = await Entity.FindAsync(id);
        Entity.Remove(entity);
    }
    public async Task DeleteByIntIdAsync(int id)
    {
        var entity = await Entity.FindAsync(id);

        Entity.Remove(entity);
    }

    public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        var entity = await GetQuery(true).FirstOrDefaultAsync(expression, cancellationToken);
        Entity.Remove(entity);
    }
    private IQueryable<TEntity> GetQuery(bool isTrackingActive)
    {
        IQueryable<TEntity> query = Entity;

        if (!isTrackingActive) query = query.AsNoTracking();

        return query;
    }
}
