using Enterprise.SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{
    // 1. الواجهة الأساسية (تُستخدم مع كل الجداول بلا استثناء)
    public interface IReadByIdService<TStandardDto,TKey> : IEntityService<TKey>
    {
        Task<Result<TStandardDto>> GetByIdAsync(TKey id);
    }
}
