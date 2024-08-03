using FinTract_REST_API.Models;

namespace FinTract_REST_API.Interfaces
{
    public interface ITransactionServices
    {
        Task<bool> ProcessTransaction(int userid, int amount, int accountid);
        Task<IEnumerable<History>> GetHistory(string userid);
    }
}
