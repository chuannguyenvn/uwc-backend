namespace SharedLibrary.Models;

public class Account
{
    public int ID { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
}