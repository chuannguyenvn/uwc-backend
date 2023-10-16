using System;
using System.Collections.Generic;
using Commons.Categories;

namespace Commons.Models
{
    public class Account : IndexedEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public UserRole UserRole { get; set; }
        
        public bool UseFaceRecognition { get; set; }
        public List<float>? FaceVector { get; set; }
    }
}