using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data.Model.Request
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
    }
}
