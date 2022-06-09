using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task LoginAsync(string email, string password);
    }
}
