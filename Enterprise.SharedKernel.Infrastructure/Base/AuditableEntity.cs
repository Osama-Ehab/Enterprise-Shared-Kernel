using Enterprise.SharedKernel.Interfaces.Models;

namespace SchoolERP.Core.Entities.Base
{
    // Now we take TWO types:
    // TKey     = The Entity's ID (e.g. OrderID)
    // TUserKey = The User's ID (e.g. CreatedBy which is an int)
    public abstract class AuditableEntity<TKey, TUserKey> : Entity<TKey>, IAuditableEntity<TKey, TUserKey>
        where TUserKey : struct
{
    public DateTime CreatedDate { get; set; }
    public TUserKey CreatedBy { get; set; } // Flexible!

    public Nullable<DateTime> ModifiedDate { get; set; }
    public Nullable<TUserKey> ModifiedBy { get; set; }
}
}
