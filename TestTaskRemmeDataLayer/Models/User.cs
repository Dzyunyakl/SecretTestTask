using System.Collections;
using System.Collections.Generic;

namespace TestTaskRemmeDataLayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public ICollection<Todo> Todos { get; set; } 
    }
}