using Newtonsoft.Json;
using PaymentsBackend.DataStore;

namespace PaymentsBackend.DataModels
{
    public class LoggingService : ILoggingService
    {
        private readonly AppDbContext _db;

        public LoggingService(AppDbContext db) => _db = db;

        public async Task LogAsync(string userId, string action, object? request, object? response, bool success, string errorMsg = "")
        {
            var log = new PaymentLog
            {
                UserId = userId,
                Action = action,
                RequestJson = request == null ? null : JsonConvert.SerializeObject(request),
                ResponseJson = response == null ? null : JsonConvert.SerializeObject(response),
                IsSuccess = success,
                ErrorMessage = errorMsg,
                CreatedAt = DateTime.UtcNow
            };

            _db.PaymentLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }

}
