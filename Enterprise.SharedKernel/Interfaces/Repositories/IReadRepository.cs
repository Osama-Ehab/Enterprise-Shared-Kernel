using Enterprise.SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces.Repositories
{
    public interface IReadRepository<TEntity, TKey>
    {
        // كل هذه الدوال يجب أن تُنفذ باستخدام .AsNoTracking() في الـ BaseRepository
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();

        // دالة سحرية تقبل التحويل لأي نوع DTO
        Task<List<TDto>> GetAllProjectedAsync<TDto>(Expression<Func<TEntity, TDto>> selectExpression);

        Task<PagedListDTO<TDto>> GetPagedProjectedAsync<TDto>(
  Expression<Func<TEntity, TDto>> selectExpression,
  int pageNumber,
  int pageSize,
  Expression<Func<TEntity, bool>>? filter = null,
  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        Task<TDto> GetByIdProjectedAsync<TDto>(TKey id, Expression<Func<TEntity, TDto>> selectExpression);

        Task<TDto> GetProjectedAsync<TDto>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TDto>> selectExpression);

    }
}
