using System;
using TestTaskRemmeDataLayer.Models;
using System.Linq;

namespace TestTaskRemmeDataLayer.Database
{
    public class DatabaseSeeder
    {
        private readonly AppContext _context;

        public DatabaseSeeder(AppContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            if (!_context.Users.Any())
            {
                _context.Users.AddRange(PredefinedData.Users);
                _context.SaveChanges();
                var user1 = _context.Users.FirstOrDefault(i => i.Name == "lev");
                var user2 = _context.Users.FirstOrDefault(i => i.Name == "anna");
                if (user1 == null || user2 == null)
                    throw new Exception("didn't find user1 and user2");
                _context.Tasks.AddRange(
                    new Task { User = user1, Info = "first task for user1" },
                    new Task { User = user2, Info = "first task for user2" },
                    new Task { User = user1, Info = "second task for user1" },
                    new Task { User = user2, Info = "second task for user2" }
                );
                _context.SaveChanges();
                var user11 = _context.Users.FirstOrDefault(u => u.Name == "lev");
                var user22 = _context.Users.FirstOrDefault(u => u.Name == "anna");
            }
        }
    }
}