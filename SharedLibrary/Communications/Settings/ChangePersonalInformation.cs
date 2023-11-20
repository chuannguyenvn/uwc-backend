using System;
using Commons.Categories;

namespace Commons.Communications.Settings
{
    public class ChangePersonalInformationRequest
    {
        public int AccountId { get; set; }

        public UserRole? UserRole { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
    }
}