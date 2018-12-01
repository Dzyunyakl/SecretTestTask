namespace TestTaskRemmeDataLayer.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public bool IsDone { get; set; } 
        public User User { get; set; }
    }
}