using Enterprise.SharedKernel.Interfaces;
using Enterprise.SharedKernel.Interfaces.Models;
using Enterprise.SharedKernel.Interfaces.Repositories;
using Enterprise.SharedKernel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Infrastructure.Repositories
{
    public class BaseReadRepository<TEntity, TKey> : IReadRepository<TEntity, TKey>
        where TEntity : class ,IEntity<TKey>
    {
    
        // نعتمد على DbContext العام الخاص بـ EF Core، وليس الخاص بمشروع معين
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseReadRepository(DbContext dbContext)
        {
            _dbContext = dbContext;

            // هذه الدالة السحرية تحدد الجدول الصحيح بناءً على نوع الـ Entity المُمرر
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            // القراءة فقط: نستخدم AsNoTracking() لقتل المراقبة وتوفير الذاكرة
            // ملاحظة: مع AsNoTracking لا يمكننا استخدام FindAsync، فنستخدم SingleOrDefault
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => EF.Property<TKey>(e.Id, "Id").Equals(id));
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            // AsNoTracking ممتازة للأداء في عمليات القراءة لأنها توقف الـ ChangeTracker
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<List<TDto>> GetAllProjectedAsync<TDto>(Expression<Func<TEntity, TDto>> selectExpression)
        {
            // 1. AsNoTracking: لا نحتاج ChangeTracker لأننا نقرأ فقط.
            // 2. Select: EF Core سيأخذ الـ Expression ويحوله إلى (SELECT Column1, Column2 FROM Table)
            return await _dbSet
                .AsNoTracking()
                .Select(selectExpression) // التحويل (Projection) يحدث هنا داخل محرك قاعدة البيانات!
                .ToListAsync();
        }

        public virtual async Task<PagedListDTO<TDto>> GetPagedProjectedAsync<TDto>(
    Expression<Func<TEntity, TDto>> selectExpression,
    int pageNumber,
    int pageSize,
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            // 1. نبدأ الاستعلام بدون تتبع (للقراءة فقط)
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            // 2. تطبيق الفلترة (Where) إن وُجدت
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // 3. 🚨 خطوة حرجة جداً: حساب العدد الإجمالي قبل عمل التقسيم (Skip/Take)
            int totalCount = await query.CountAsync();

            // 4. تطبيق الترتيب (Order By) - إجباري في SQL قبل التقسيم
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            // 5. تطبيق التقسيم (Pagination) والتحويل (Projection)
            // لاحظ: Select تُنفذ في النهاية لكي يتم تحويل الأعمدة المطلوبة فقط
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(selectExpression)
                .ToListAsync();

            // 6. تجميع النتيجة النهائية
            return new PagedListDTO<TDto>(items, totalCount, pageNumber, pageSize);
        }


        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }
        public virtual async Task<TDto> GetByIdProjectedAsync<TDto>(TKey id, Expression<Func<TEntity, TDto>> selectExpression)
        {
            // القراءة فقط: نستخدم AsNoTracking() لقتل المراقبة وتوفير الذاكرة
            // ملاحظة: مع AsNoTracking لا يمكننا استخدام FindAsync، فنستخدم SingleOrDefault
            return await _dbSet.AsNoTracking().Select(selectExpression).FirstOrDefaultAsync(e => EF.Property<TKey>(e, "Id").Equals(id));
        }

        public virtual async Task<TDto> GetProjectedAsync<TDto>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TDto>> selectExpression)
        {
            // 1. AsNoTracking: لا نحتاج ChangeTracker لأننا نقرأ فقط.
            // 2. Select: EF Core سيأخذ الـ Expression ويحوله إلى (SELECT Column1, Column2 FROM Table)
            return await _dbSet
                .AsNoTracking()
                .Where(expression)
                .Select(selectExpression) // التحويل (Projection) يحدث هنا داخل محرك قاعدة البيانات!
                .FirstOrDefaultAsync();
               
        }
    }

}
