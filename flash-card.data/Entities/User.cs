
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace flash_card.data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
