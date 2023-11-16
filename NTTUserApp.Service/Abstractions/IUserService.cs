using NTTUserApp.Service.Models.Users;

namespace NTTUserApp.Service.Abstractions;
public interface IUserService
{
    Task<List<UserModel>> GetUsersAsync();
    Task<UserModel> GetUserByIdAsync(GetUserByIdRequest request);
    Task<UserModel> CreateUserAsync(CreateUserRequest request);
    Task<UserModel> UpdateUserAsync(UpdateUserRequest request);
    Task<bool> DeleteUserAsync(DeleteUserRequest request);
}
