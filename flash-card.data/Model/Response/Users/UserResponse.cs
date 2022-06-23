using flash_card.data.Entities;

namespace flash_card.data.Model.Response.Users
{
    public class UserResponse
    {
        public User User { get; set; }
        public string JwtToken { get; set; }

        //public UserResponse(Entities.User user)
        //{
        //    User = new Entities.User
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        PhoneNumber = user.PhoneNumber,
        //        Email = user.Email,
        //    };
        //}

        //public UserResponse(Entities.User user, string jwtToken)
        //{
        //    User = new Entities.User
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        PhoneNumber = user.PhoneNumber,
        //        Email = user.Email,
        //    };
        //    JwtToken = jwtToken;
        //}
    }
}
