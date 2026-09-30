using Enterprise.SharedKernel.Interfaces;
using Enterprise.SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{
    public interface IUpdateService<TUpdateDto> : IService
    {
        Task<Result<bool>> UpdateAsync(TUpdateDto dto);
    }
}
