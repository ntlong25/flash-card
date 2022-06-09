using flash_card.data;
using flash_card.data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task LoginAsync(string email, string password)
        {
            await Task.FromResult(_dataContext.Users.SingleOrDefault(e => e.Email.Equals(email) && e.PasswordHash.Equals(password)));
        }
    }
}
