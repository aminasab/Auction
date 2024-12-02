using OnlineAuction.Models;

namespace OnlineAuction.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByIdAsync(int id);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task AddUserAsync(UserModel user);
        Task<UserModel> GetUserByFullNameAsync(string firstName, string lastName);
        Task<UserModel> GetUserByFullEmailAsync(string email);
    }
}
