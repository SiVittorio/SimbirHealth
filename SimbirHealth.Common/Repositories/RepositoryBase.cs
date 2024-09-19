using Microsoft.EntityFrameworkCore;
using SimbirHealth.Common.Interfaces.Repositories;
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
        public async Task<List<TEntity>> GetListAsync() => await _db.Set<TEntity>().ToListAsync();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken token) => await _db.SaveChangesAsync(token);
    }
}
