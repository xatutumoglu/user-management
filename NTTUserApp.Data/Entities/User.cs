using NTTUserApp.Data.Entities.Base;

namespace NTTUserApp.Data.Entities;
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Mail { get; set; }
    public string TRNumber { get; set; }
    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }

}