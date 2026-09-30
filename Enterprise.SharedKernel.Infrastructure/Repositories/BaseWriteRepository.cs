using Enterprise.SharedKernel.Interfaces;
using Enterprise.SharedKernel.Interfaces.Models;
using Enterprise.SharedKernel.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Infrastructure.Repositories
{

    public class BaseWriteRepository<TEntity, TKey> : IWriteRepository<TEntity, TKey>
          where TEntity : class, IEntity<TKey>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
       
        protected readonly ICurrentUser _currentUserContext;


    
        public BaseWriteRepository(DbContext dbContext,ICurrentUser currentUser)
        {
            _currentUserContext = currentUser;

            _dbContext = dbContext;

            // هذه الدالة السحرية تحدد الجدول الصحيح بناءً على نوع الـ Entity المُمرر
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity?> GetByIdAsync(TKey id)
        {
            // الكتابة: نستخدم FindAsync لأنها ذكية جداً وتضع الكيان تحت المراقبة (ChangeTracker) فوراً!
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<TKey> AddAsync(TEntity entity)
        {
            // 1. Audit Data Injection
            if (entity is IAuditableEntity<TKey, int> auditableEntity)
            {
                auditableEntity.CreatedDate = DateTime.UtcNow;
                auditableEntity.CreatedBy = _currentUserContext.UserId;
            }

            // 2. Add and Save
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            // 3. Returning the ID directly (Thanks to the IEntity constraint)
            return entity.Id;
        }

      
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity is IAuditableEntity<TKey, int> auditableEntity)
            {
                auditableEntity.ModifiedDate = DateTime.UtcNow;
                //auditableEntity.ModifiedBy = _currentUserContext.UserId;
            }

            var RowsEfected = await _dbContext.SaveChangesAsync();
            return RowsEfected > 0;
        }

        public virtual async Task<bool> DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                var RowsEfected = await _dbContext.SaveChangesAsync();
                return RowsEfected > 0;
            }
            return false;
        }
    }
}
