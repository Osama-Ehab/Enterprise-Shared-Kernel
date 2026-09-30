using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces.Models
{
    public interface IAuditableEntity<TKey, TUserKey> : IEntity<TKey>
        where TUserKey : struct
    {
        DateTime CreatedDate { get; set; }
        TUserKey CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        Nullable<TUserKey> ModifiedBy { get; set; }
    }


}
