using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using SportsCenterReservation.Domain.Core;

namespace SportsCenterReservation.Domain.Repository;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}
