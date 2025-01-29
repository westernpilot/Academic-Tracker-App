using SQLite;

namespace c971.Shared.Models
{
    public abstract class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string name;
        private string email;

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be empty");
                }
                name = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                {
                    throw new ArgumentException("Invalid email address");
                }
                email = value;
            }
        }

        public abstract string GetUserType();
    }
}
