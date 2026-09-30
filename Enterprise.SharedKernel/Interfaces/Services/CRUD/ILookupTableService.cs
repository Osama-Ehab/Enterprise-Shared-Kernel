using Enterprise.SharedKernel.Interfaces.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces.CRUD
{
    // الواجهة المجمعة (Composite Interface)
    public interface ILookupTableService<TStandardDTO, TKey, TCreateDto, TUpdateDto, TLookupDTO, TListDTO> :
        ICommandService<TCreateDto, TUpdateDto, TKey>,
        ILookupTableQueryService<TStandardDTO, TKey, TLookupDTO, TListDTO> where TLookupDTO : class
    {
        // 💎 تبقى هذه الواجهة فارغة تماماً!
        // وظيفتها الوحيدة هي تجميع الواجهات الستة في عقد واحد.
    }
}
