using Enterprise.SharedKernel.DTOs.Interfaces;
using Enterprise.SharedKernel.Interfaces.Models;
using Enterprise.SharedKernel.Models; // بافتراض وجود كلاس Result هنا
using System.Linq.Expressions;

namespace Enterprise.Application.Interfaces.Services
{
    public interface IGenericService<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        // عمليات القراءة (ترجع DTOs عبر الـ Projection)
        Task<Result<TDto>> GetByIdProjectedAsync<TDto>(TKey id, Expression<Func<TEntity, TDto>> selectExpression);
        Task<Result<List<TDto>>> GetAllProjectedAsync<TDto>(Expression<Func<TEntity, TDto>> selectExpression);

        // عمليات الكتابة (تقبل Entity، وسيتم تحويل الـ DTO إليها في طبقة الـ UI أو عبر AutoMapper)
        Task<Result<TKey>> AddAsync<TCreateDTO>(TCreateDTO dto) where TCreateDTO : class;

        Task<Result> UpdateAsync<TUpdateDTO>(TUpdateDTO dto) where TUpdateDTO : class, IIdentifiableDto<TKey>;

        Task<Result> DeleteAsync(TKey id);
    }
}