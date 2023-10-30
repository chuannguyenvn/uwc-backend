using System;
using Commons.Categories;

namespace Commons.Communications.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }

    public class RegisterResponse
    {
        public Credentials Credentials { get; set; }
        public InitializationData InitializationData { get; set; }
    }
}