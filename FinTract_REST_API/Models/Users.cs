namespace FinTract_REST_API.Models
{
    public class Users
    {
        public int? id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public int? WalletBalance { get; set; }
    }
}
