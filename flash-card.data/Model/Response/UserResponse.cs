using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data.Model.Response
{
    public class UserResponse
    {
        public User User { get; set; }
        public string JwtToken { get; set; }

        public UserResponse(User user)
        {
            this.User = new User
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
            };
        }

        public UserResponse(User user, string jwtToken)
        {
            this.User = new User
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
            };
            JwtToken = jwtToken;
        }
    }
}
