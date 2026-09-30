using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces.Repositories
{
    public interface IWriteRepository<TEntity, TKey>
    {
        // هذه الدوال تعتمد على الـ ChangeTracker

        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TKey> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TKey id);

    }
}
