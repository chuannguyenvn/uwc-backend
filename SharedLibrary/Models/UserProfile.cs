﻿namespace SharedLibrary.Models;

public class UserProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    
    public int AccountID { get; set; }
    public Account Account { get; set; }
}