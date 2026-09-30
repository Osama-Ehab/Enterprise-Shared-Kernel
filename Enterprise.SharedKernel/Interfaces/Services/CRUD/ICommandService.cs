using Enterprise.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{
    public interface ICommandService<TCreateDto, TUpdateDto, TKey> :
      ICreateService<TCreateDto, TKey>,
      IUpdateService<TUpdateDto>,
      IDeleteService<TKey>
    {
    }
}
