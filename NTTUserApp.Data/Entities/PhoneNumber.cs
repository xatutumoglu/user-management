using NTTUserApp.Data.Entities.Base;

namespace NTTUserApp.Data.Entities;
public class PhoneNumber : BaseEntity
{
    public string PhoneNo { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }

}