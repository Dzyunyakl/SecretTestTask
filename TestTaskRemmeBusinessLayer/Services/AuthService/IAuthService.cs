using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeDataLayer.Models;

namespace TestTaskRemmeBusinessLayer.Services.AuthService
{
    public interface IAuthService
    {
        OperationResult<User> Authorize(int id);
    }
}