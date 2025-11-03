using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace VelocipedeUtils.Ubp.Core.Repositories;

/// <summary>
/// Allows to interact with the initial collections.
/// </summary>
public class GenericRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Initial dataset.
    /// </summary>
    private List<TEntity> dbSet;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public GenericRepository()
    {
        this.dbSet = new List<TEntity>();
    }

    /// <summary>
    /// Gets a collection after applying the specified filtered.
    /// </summary>
    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
    {
        IQueryable<TEntity> query = dbSet.AsQueryable();
        if (filter != null)
            query = query.Where(filter);
        if (orderBy != null)
            return orderBy(query).ToList();
        return query.ToList();
    }

    /// <summary>
    /// Adds an object to the dataset.
    /// </summary>
    public virtual void Insert(TEntity entity)
    {
        if (entity == null)
            throw new System.Exception("Entity could not be null");

        dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
    }

    public virtual void Delete(TEntity entityToDelete)
    {
    }

    public virtual void Update(TEntity entityToUpdate)
    {
    }
}