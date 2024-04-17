namespace Shortify.Client.Data.Models
{
    public class User
    {
        public User()
        {
            Url = new List<Url>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public List<Url> Url { get; set; }
    }
}
