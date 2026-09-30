using Enterprise.SharedKernel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.DTOs.Interfaces
{
    public interface IFilterDTO
    {
        public string LangCode { get; }  
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
