using Microsoft.EntityFrameworkCore;
using PaymentsBackend.DataModels;

namespace PaymentsBackend.DataStore
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db) => _db = db;

        public Task<List<Payment>> GetByUserAsync(string userId) =>
            _db.Payments.Where(p => p.UserId == userId)
                        .OrderByDescending(p => p.CreatedAt)
                        .ToListAsync();

        public Task<List<Payment>> GetAllAsync() =>
            _db.Payments.OrderByDescending(p => p.CreatedAt).ToListAsync();

        public Task<Payment?> GetByIdAsync(string userId, int id) =>
            _db.Payments.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == id);

        public Task<Payment?> GetDuplicatePaymentAsync(string userId, int id) =>
            _db.Payments.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == id);

        public async Task AddAsync(Payment payment)
        {
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _db.Payments.Update(payment);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Payment payment)
        {
            _db.Payments.Remove(payment);
            await _db.SaveChangesAsync();
        }
    }

}
