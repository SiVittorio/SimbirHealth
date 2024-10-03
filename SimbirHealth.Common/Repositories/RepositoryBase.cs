using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
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
        public void Delete(TEntity entity) => _db.Remove(entity);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IQueryable<TEntity> Query() => _db.Set<TEntity>().AsQueryable<TEntity>();

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
