using OnlineAuction.Models;

namespace OnlineAuction.Interfaces
{
    public interface IMandarinRepository
    {
        Task<Mandarin> GetMandarinByIdAsync(int id);
        Task<IEnumerable<Mandarin>> GetAllMandarinsAsync();
        Task AddMandarinAsync(Mandarin mandarin);
        Task UpdateFinalPriceMandarinAsync(int id, decimal price);
        Task DeleteMandarinAsync(int id);
    }
}
