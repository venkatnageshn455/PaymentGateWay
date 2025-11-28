namespace PaymentsBackend.DataModels
{
    public class DailySequence
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int LastSeq { get; set; }
    }
}
