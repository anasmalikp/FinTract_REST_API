namespace FinTract_REST_API.Models
{
    public class History
    {
        public int? id { get; set; }
        public string? transactionName { get; set; }
        public DateTime? transactionTime { get; set; }
        public int? userid { get; set; }
        public int? categoryid { get; set; }
        public int? Amt { get; set; }
    }
}
