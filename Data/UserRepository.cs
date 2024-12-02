using Microsoft.EntityFrameworkCore;
using OnlineAuction.Interfaces;
using OnlineAuction.Models;

namespace OnlineAuction.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<UserModel> GetUserByFullNameAsync(string firstName, string lastName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(user => user.FirstName == firstName && user.LastName == lastName);
        }

        public async Task<UserModel> GetUserByFullEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
