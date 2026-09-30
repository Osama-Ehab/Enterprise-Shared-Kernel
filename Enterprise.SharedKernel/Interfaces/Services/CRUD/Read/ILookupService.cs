using Enterprise.SharedKernel.Models;
using Enterprise.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{
    // ====================================================
    // LOOKUP SERVICE (For static reference tables)
    // ====================================================
    public interface ILookupService<TLookupDTO> : IService where TLookupDTO : class
    {
        Task<Result<IReadOnlyList<TLookupDTO>>> GetAllLookupAsync();
    }
}
