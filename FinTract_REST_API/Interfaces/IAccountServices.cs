using FinTract_REST_API.Models;

namespace FinTract_REST_API.Interfaces
{
    public interface IAccountServices
    {
        Task<bool> CreateAccount(Accounts account);
        Task<IEnumerable<Accounts>> GetAllAccount();
    }
}
