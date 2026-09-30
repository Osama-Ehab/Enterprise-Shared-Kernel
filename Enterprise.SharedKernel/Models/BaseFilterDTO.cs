using Enterprise.SharedKernel.Context;
using Enterprise.SharedKernel.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Models
{
    // Base Class (موجود في Core Project)
    public abstract class BaseFilterDTO : IFilterDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string LangCode => AppLayoutContext.IsArabicLayout ? "ar" : "en";
    }
}
