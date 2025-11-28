using Microsoft.EntityFrameworkCore;
using PaymentsBackend.DataStore;

namespace PaymentsBackend.DataModels
{
    public class DailySequenceRepository : IDailySequenceRepository
    {
        private readonly AppDbContext _db;

        public DailySequenceRepository(AppDbContext db) => _db = db;

        public async Task<int> GetNextSequenceAsync(string userId, DateTime date)
        {
            var seq = await _db.DailySequences
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Date == date);

            if (seq == null)
            {
                seq = new DailySequence
                {
                    UserId = userId,
                    Date = date,
                    LastSeq = 1
                };
                _db.DailySequences.Add(seq);
            }
            else
            {
                seq.LastSeq++;
            }

            await _db.SaveChangesAsync();
            return seq.LastSeq;
        }
    }

}
