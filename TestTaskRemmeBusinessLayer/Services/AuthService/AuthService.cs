using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task<OperationResult<User>> Authorize(int id)
        {
            var users = await _db.Users.ToListAsync();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return OperationResult<User>.Unauthorized();
            
            return OperationResult<User>.Ok(user);
        }
    }
}