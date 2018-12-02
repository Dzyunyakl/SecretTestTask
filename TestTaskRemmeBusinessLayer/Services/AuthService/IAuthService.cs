using System.Threading.Tasks;
using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeDataLayer.Models;

namespace TestTaskRemmeBusinessLayer.Services.AuthService
{
    public interface IAuthService
    {
        Task<OperationResult<User>> Authorize(int id);
    }
}