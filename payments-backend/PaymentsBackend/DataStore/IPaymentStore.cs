using PaymentsBackend.DataModels;

namespace PaymentsBackend.DataStore
{
    public interface IPaymentStore
    {
        Task<ApiResponse> GetUserPaymentsAsync(string userId);
        Task<ApiResponse> GetAllPaymentsAsync();
        Task<ApiResponse> GetPaymentByIdAsync(string userId, int id);
        Task<ApiResponse> CreatePaymentAsync(PaymentRequestDto request);
        Task<ApiResponse> UpdatePaymentAsync(PaymentRequestDto request);
        Task<ApiResponse> DeletePaymentAsync(string userId, int id);
        Task<ApiResponse> GetAllUsersAsync();
    }
}
