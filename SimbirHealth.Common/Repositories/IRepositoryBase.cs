using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Common.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        /// <summary>
        /// Создать запрос
        /// </summary>
        /// <returns>Список сущностей</returns>
        IQueryable<TEntity> Query();
        /// <summary>
        /// Добавить сущность в БД
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Кол-во записанных элементов</returns>
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
