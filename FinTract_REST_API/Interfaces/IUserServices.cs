using FinTract_REST_API.Models;

namespace FinTract_REST_API.Interfaces
{
    public interface IUserServices
    {
        Task<int?> RegisterUser(Users user);
        Task<string> LoginUser(Users user);
    }
}
