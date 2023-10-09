using System;
#if NET7_0
using System.ComponentModel.DataAnnotations.Schema;
#endif
using Commons.Categories;

namespace Commons.Models
{
    public class UserProfile : IndexedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public UserRole UserRole { get; set; }

#if NET7_0
        [ForeignKey("Account")]
#endif
        public int? AccountID { get; set; }
        public Account Account { get; set; }
    }
}