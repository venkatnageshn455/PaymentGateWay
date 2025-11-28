namespace PaymentsBackend.DataStore
{
    public interface IDailySequenceRepository
    {
        Task<int> GetNextSequenceAsync(string userId, DateTime date);
    }
}
