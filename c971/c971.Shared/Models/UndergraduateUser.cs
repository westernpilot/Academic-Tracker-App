namespace c971.Shared.Models
{
    public class UndergraduateUser : User
    {
        public override string GetUserType()
        {
            return "Undergraduate";
        }
    }
}
