using NTTUserApp.Data.Entities;

namespace NTTUserApp.Service.Models.Users;
public class UserModel
{
    public UserModel()
    {
        
    }
    public UserModel(User user = null)
    {
        if (user == null)
        {
            return;
        }

        Id = user.Id;
        Name = user.Name;
        Mail = user.Mail;
        TRNumber = user.TRNumber;
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mail { get; set; }
    public string TRNumber { get; set; }
}