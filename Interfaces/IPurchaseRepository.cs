using OnlineAuction.Models;

namespace OnlineAuction.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase> GetByIdAsync(int purchaseId);
        Task<IEnumerable<Purchase>> GetAllAsync();
        Task AddAsync(Purchase purchase);
        Task UpdateAsync(Purchase purchase);
    }
}
