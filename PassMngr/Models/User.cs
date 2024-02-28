namespace PassMngr.Models
{
    public class User
    {
        public int id { get; set; }
        public string? email { get; set; }

        public string? password { get; set; }

        public string? name { get; set; }
        public string? surname { get; set; }

        public string date_of_registration { get; set; }

        public bool? is_active { get; set; } = null;

        public bool? is_confirmed { get; set; } = null;

        public List<Password> password_list { get; set; } = new List<Password>();

        public User() { } 
        
        public User(int id, string email, string password, string name, string surname, string date_of_registration, bool is_active, bool is_confirmed/*, List<Password> password_list = default*/)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.name = name;
            this.surname = surname;
            this.date_of_registration = date_of_registration;
            this.is_active = is_active;
            this.is_confirmed = is_confirmed;
            //this.password_list = password_list;
        }
        
    }
}
