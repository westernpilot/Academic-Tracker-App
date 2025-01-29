using SQLite;

namespace c971.Shared.Models
{
    public class GraduateUser : User
    {
        // Constructor is necessary for SQLite to create instances
        public GraduateUser()
        {
        }

        // Method to return the user type
        public override string GetUserType()
        {
            return "Graduate";
        }
    }
}
