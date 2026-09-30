using Enterprise.SharedKernel.DTOs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Models
{
    public abstract class BaseIdentifiableDTO<TKey> : IIdentifiableDto<TKey>
    {
        // 1. The Developer's Property (Strongly Typed!)
        public TKey Id { get; set; }

        // 2. The Router's Property (Explicit Implementation)
        // When the Router looks at this DTO as an 'IIdentifiableDto', it gets the boxed object.
        object IIdentifiableDto.Id => Id;
    }
}