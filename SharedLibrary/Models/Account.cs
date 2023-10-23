namespace Commons.Models
{
    public class Account : IndexedEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}