using flash_card.data;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly DataContext _dataContext;
        public TopicRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
        
    }
}
