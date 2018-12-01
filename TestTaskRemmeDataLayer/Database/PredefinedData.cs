using TestTaskRemmeDataLayer.Models;

namespace TestTaskRemmeDataLayer.Database
{
    public class PredefinedData
    {
        public static User[] Users = {
            new User { Name = "lev", Password = "password1", Role = 0 },
            new User { Name = "anna", Password = "password2", Role = 1 }
        };
    }
}