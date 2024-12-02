using Microsoft.EntityFrameworkCore;
using OnlineAuction.Interfaces;
using OnlineAuction.Models;

namespace OnlineAuction.Data
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly MyDbContext _context;

        public PurchaseRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Purchase purchase)
        {
            if (purchase == null) throw new ArgumentNullException(nameof(purchase));
            await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            return await _context.Purchases.ToListAsync();
        }

        public async Task<Purchase> GetByIdAsync(int purchaseId)
        {
            var purchase = await _context.Purchases.FindAsync(purchaseId);
            if (purchase == null) throw new KeyNotFoundException($"Покупка с ID {purchaseId} не найдена");
            return purchase;
        }

        public async Task UpdateAsync(Purchase purchase)
        {
            if (purchase == null) throw new ArgumentNullException(nameof(purchase));
            var existingPurchase = await _context.Purchases.FindAsync(purchase.PurchaseId);
            if (existingPurchase == null) throw new KeyNotFoundException($"Покупка с ID {purchase.PurchaseId} не найдена");
            existingPurchase.PurchasePrice = purchase.PurchasePrice; 
            await _context.SaveChangesAsync();
        }
    }
}
