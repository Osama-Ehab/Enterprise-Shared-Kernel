using Enterprise.SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{

    // ====================================================
    // DETAILS READ: Requires TKey
    // ====================================================
    // It inherits IEntityService because fetching details requires the ID.
    public interface IReadDetailsByIdService<TDetailsDTO, TKey> : IEntityService<TKey>
    {
        Task<Result<TDetailsDTO>> GetDetailsAsync(TKey id);
    }

}
