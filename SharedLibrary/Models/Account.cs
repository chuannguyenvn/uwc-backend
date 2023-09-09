namespace Commons.Models;

public class Account : IndexedEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}