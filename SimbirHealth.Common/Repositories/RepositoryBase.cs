using Microsoft.EntityFrameworkCore;
using SimbirHealth.Data.Models._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class, IDeleteable
    {
        private readonly SimbirHealthContext _db;

        public RepositoryBase(SimbirHealthContext context)
        {
            _db = context;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TEntity Add(TEntity entity) => _db.Add(entity).Entity;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void AddRange(List<TEntity> entities) => _db.AddRange(entities);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Delete(TEntity entity) => _db.Remove(entity);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void DeleteRange(List<TEntity> entities) => _db.RemoveRange(entities);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IQueryable<TEntity> Query() => _db.Set<TEntity>().AsQueryable<TEntity>().Where(e => !e.IsDeleted);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IQueryable<TEntity> QueryWithDeleted() => _db.Set<TEntity>().AsQueryable<TEntity>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken token = default) => await _db.SaveChangesAsync(token);
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TEntity Update(TEntity updatedEntity) => _db.Update(updatedEntity).Entity;
    }
}
