using Enterprise.SharedKernel.Models;
using Enterprise.SharedKernel.DTOs.Interfaces;

namespace Enterprise.SharedKernel.Interfaces
{
    // ====================================================
    // LIST READ: Pure ISP Compliance. NO TKEY!
    // ====================================================
    // It inherits the base IService, so DI still finds it, but it drops TKey.
    public interface IPagedListService<TListDTO> : IService 
    {
        Task<Result<PagedListDTO<TListDTO>>> GetListAsync(IFilterDTO filterDTO);

    }

}
