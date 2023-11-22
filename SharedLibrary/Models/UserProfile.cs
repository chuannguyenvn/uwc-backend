using System;
using System.Drawing;
using Commons.Categories;

namespace Commons.Models
{
    public class UserProfile : IndexedEntity
    {
        public UserRole UserRole { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public float AvatarColorHue { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}