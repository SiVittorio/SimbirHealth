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
        IQueryable<TEntity> Query();
        /// <summary>
        /// Добавить сущность в БД
        /// </summary>
        TEntity Add(TEntity entity);
        /// <summary>
        /// Удалить сущность из БД
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Кол-во записанных элементов</returns>
        Task<int> SaveChangesAsync(CancellationToken token = default);
        /// <summary>
        /// Обновить сущность
        /// </summary>
        /// <param name="updatedEntity">Обновленная версия сущности</param>
        /// <returns></returns>
        TEntity Update(TEntity updatedEntity);
    }
}
