using PaymentsBackend.DataModels;

namespace PaymentsBackend.DataStore
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetByUserAsync(string userId);
        Task<List<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(string userId, int id);
        Task<Payment?> GetDuplicatePaymentAsync(string userId, int id);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(Payment payment);
    }
}
