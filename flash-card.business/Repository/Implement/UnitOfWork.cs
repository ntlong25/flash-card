using flash_card.data;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        public IRepository<User> UserRepository { get; }
        public IRepository<Role> RoleRepository { get; }
        public IRepository<Topic> TopicRepository { get; }
        public IRepository<FlashCard> CardRepository { get; }
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            UserRepository = new Repository<User>(_dataContext);
            RoleRepository = new Repository<Role>(_dataContext);
            TopicRepository = new Repository<Topic>(_dataContext);
            CardRepository = new Repository<FlashCard>(_dataContext);
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public async Task CompleteAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
