using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.SharedKernel.Models
{
    /// <summary>
    /// حاوية بيانات خام لنقل القوائم المجزأة وإجمالي العدد من السيرفر إلى الشاشة
    /// </summary>
    public class PagedListDTO<TListDTO> 
    {
        /// <summary>
        /// قائمة السجلات للصفحة الحالية
        /// </summary>
        public IReadOnlyList<TListDTO> Data { get; set; } = Array.Empty<TListDTO>();

        /// <summary>
        /// إجمالي عدد السجلات في قاعدة البيانات (لإرساله إلى أداة الترقيم)
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// رقم الصفحة الحالية
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// حجم الصفحة
        /// </summary>
        public int PageSize { get; set; }

        // حساب إجمالي الصفحات أوتوماتيكياً
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public PagedListDTO() { }

        public PagedListDTO(IEnumerable<TListDTO> items, int totalCount, int pageNumber, int pageSize)
        {
            Data = items?.ToList() ?? new List<TListDTO>();
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public static PagedListDTO<TListDTO> Empty(int pageNumber = 1, int pageSize = 15)
        {
            return new PagedListDTO<TListDTO>(Enumerable.Empty<TListDTO>(), 0, pageNumber, pageSize);
        }
    }
}