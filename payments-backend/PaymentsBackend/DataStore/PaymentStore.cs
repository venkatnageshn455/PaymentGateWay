using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentsBackend.DataModels;

namespace PaymentsBackend.DataStore
{
    public class PaymentStore : IPaymentStore
    {
        private readonly IPaymentRepository _repo;
        private readonly IDailySequenceRepository _seqRepo;
        private readonly ILoggingService _logger;

        private readonly string[] AllowedCurrencies = { "INR", "USD", "EUR", "GBP" };

        public PaymentStore(
            IPaymentRepository repo,
            IDailySequenceRepository seqRepo,
            ILoggingService logger)
        {
            _repo = repo;
            _seqRepo = seqRepo;
            _logger = logger;
        }

        public async Task<ApiResponse> GetUserPaymentsAsync(string userId)
        {
            var list = await _repo.GetByUserAsync(userId);
            return new ApiResponse(list);
        }

        public async Task<ApiResponse> GetAllPaymentsAsync()
        {
            var list = await _repo.GetAllAsync();
            return new ApiResponse(list);
        }

        public async Task<ApiResponse> GetPaymentByIdAsync(string userId, int id)
        {
            var payment = await _repo.GetByIdAsync(userId, id);
            if (payment == null) return new ApiResponse("Not Found");
            return new ApiResponse(payment);
        }

        public async Task<ApiResponse> CreatePaymentAsync(PaymentRequestDto request)
        {
            // Duplicate check
            var existing = await _repo.GetDuplicatePaymentAsync(request.UserId, request.id);
            if (existing != null)
            {
                await _logger.LogAsync(request.UserId, "Create", request, existing, true, "Duplicate");
                return new ApiResponse(existing);
            }

            // Validation
            if (request.Amount <= 0) return new ApiResponse("Amount must be > 0");
            if (!AllowedCurrencies.Contains(request.Currency))
                return new ApiResponse("Invalid currency");

            // Generate sequence + reference
            var today = DateTime.UtcNow.Date;
            var nextSeq = await _seqRepo.GetNextSequenceAsync(request.UserId, today);

            var reference = $"{request.UserId}-PAY-{today:yyyyMMdd}-{nextSeq:D4}";

            var payment = new Payment
            {
                Id = request.id,
                UserId = request.UserId,
                Amount = request.Amount,
                Currency = request.Currency,
                Reference = reference,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(payment);
            await _logger.LogAsync(request.UserId, "Create", request, payment, true);

            return new ApiResponse(payment);
        }

        public async Task<ApiResponse> UpdatePaymentAsync(PaymentRequestDto request)
        {
            var payment = await _repo.GetByIdAsync(request.UserId, request.id);
            if (payment == null) return new ApiResponse("Not Found");

            payment.Amount = request.Amount;
            payment.Currency = request.Currency;
            payment.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(payment);
            await _logger.LogAsync(request.UserId, "Update", request, payment, true);

            return new ApiResponse(payment);
        }

        public async Task<ApiResponse> DeletePaymentAsync(string userId, int id)
        {
            var payment = await _repo.GetByIdAsync(userId, id);
            if (payment == null) return new ApiResponse("Not Found");

            await _repo.DeleteAsync(payment);
            await _logger.LogAsync(userId, "Delete", new { id }, "Deleted", true);

            return new ApiResponse("Deleted");
        }

        public async Task<ApiResponse> GetAllUsersAsync()
        {
            var all = await _repo.GetAllAsync();

            var distinctUserNames = all.Select(u => u.UserId)
                                       .Distinct()
                                       .ToList();

            return new ApiResponse(distinctUserNames);
        }



    }

}
