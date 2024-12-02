using Microsoft.EntityFrameworkCore;
using Npgsql;
using OnlineAuction.Interfaces;
using OnlineAuction.Models;

namespace OnlineAuction.Data
{
    public class MandarinRepository : IMandarinRepository
    {
        private readonly MyDbContext _context;

        public MandarinRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddMandarinAsync(Mandarin mandarin)
        {
            if (mandarin == null) throw new ArgumentNullException(nameof(mandarin));
            await _context.Mandarins.AddAsync(mandarin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMandarinAsync(int id)
        {
            var mandarin = await _context.Mandarins.FindAsync(id);
            if (mandarin != null)
            {
                _context.Mandarins.Remove(mandarin);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Мандарин с ID {id} не найден.");
            }
        }

        public async Task<IEnumerable<Mandarin>> GetAllMandarinsAsync()
        {
            return await _context.Mandarins.ToListAsync();
        }

        public async Task<Mandarin> GetMandarinByIdAsync(int id)
        {
            var mandarin = await _context.Mandarins.FindAsync(id);
            if (mandarin == null) throw new KeyNotFoundException($"Мандарин с ID - {id} не найден");
            return mandarin;
        }

        public async Task UpdateFinalPriceMandarinAsync(int id, decimal price)
        {
            var existingMandarin = await _context.Mandarins.FindAsync(id);
            // Обновляем только цену.
            existingMandarin.FinalPrice = price;
            await _context.SaveChangesAsync();
        }
    }
}