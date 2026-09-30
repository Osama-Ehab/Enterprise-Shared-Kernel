using Enterprise.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces
{


    public interface ILookupTableQueryService<TStandardDTO, TKey, TLookupDTO, TListDTO> :
      IReadByIdService<TStandardDTO, TKey>,     // 1. لجلب الكيان الخام وقت التعديل
      IPagedListService<TListDTO>, // 2. لعرض الشبكة (Grid) مع التقسيم والفلترة
      ILookupService<TLookupDTO> where TLookupDTO : class
    {
    }

    public interface ICrudExtendedQueryService<TStandardDTO, TKey, TListDTO, TDetailDto>
        : IReadByIdService<TStandardDTO, TKey>,     // 1. لجلب الكيان الخام وقت التعديل
        IPagedListService<TListDTO>, // 2. لعرض الشبكة (Grid) مع التقسيم والفلترة,
        IReadDetailsByIdService<TDetailDto, TKey> // 💎 1. النجم هنا: لجلب التفاصيل العميقة

    {
    }
}
