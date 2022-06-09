using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace flash_card.data.Entities
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<User> Users { get; set; }
    }
}
