using System.Linq;
using System.Net;
using TestTaskRemmeBusinessLayer.Extensions;
using TestTaskRemmeDataLayer.Database;
using TestTaskRemmeDataLayer.Models;

namespace TestTaskRemmeBusinessLayer.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppContext _db;

        public AuthService(AppContext db)
        {
            _db = db;
        }
        public OperationResult<User> Authorize(int id)
        {
            var users = _db.Users.ToList();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return OperationResult<User>.Unauthorized();
            
            return OperationResult<User>.Ok(user);
        }
    }
}