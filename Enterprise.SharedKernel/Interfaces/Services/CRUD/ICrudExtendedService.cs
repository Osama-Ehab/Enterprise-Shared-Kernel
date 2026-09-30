using Enterprise.SharedKernel.Interfaces.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{
    // الواجهة المجمعة للكيانات المعقدة (Aggregate Roots)
    public interface ICrudExtendedService<TKey,TStandardDTO,TCreateDTO,TUpdateDTO,  TListDTO, TDetailsDTO> :
        ICommandService< TCreateDTO, TUpdateDTO, TKey>,
        ICrudExtendedQueryService<TStandardDTO, TKey, TListDTO, TDetailsDTO>
    {
        // الواجهة تبقى فارغة، مجرد حزمة (Bundle) للواجهات الستة
    }

}
