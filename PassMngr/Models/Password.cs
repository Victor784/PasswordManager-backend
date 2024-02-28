namespace PassMngr.Models
{
    public class Password
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string associated_website { get; set; }
        public string associated_email {  get; set; }
        public string password_value { get; set; }
        public string time_of_creation {  get; set; }
        public string time_of_last_update {  get; set; }
        public string? expiration_date { get; set; }

        public Password(int id, int user_id, string associated_website, string associated_email, string password_value, string time_of_creation, string time_of_last_update, string? expiration_date)
        {
            this.id = id;
            this.user_id = user_id;
            this.associated_website = associated_website;
            this.associated_email = associated_email;
            this.password_value = password_value;
            this.time_of_creation = time_of_creation;
            this.time_of_last_update = time_of_last_update;
            this.expiration_date = expiration_date;
        }

        public Password()
        {
            this.id = 0;
            this.user_id = 0;
            this.associated_website = "";
            this.associated_email = "";
            this.password_value = "";
            this.time_of_creation = "";
            this.time_of_last_update = "";
            this.expiration_date = "";
        }
    }
}
