using flash_card.data;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly DataContext _dataContext;
        public RoleRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
