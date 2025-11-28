namespace PaymentsBackend.DataStore
{
    public interface ILoggingService
    {
        Task LogAsync(string userId, string action, object? request, object? response, bool success, string errorMsg = "");
    }

}
