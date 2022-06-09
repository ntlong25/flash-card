using flash_card.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        public IUserRepository UserRepository { get; }
        public ITopicRepository TopicRepository { get; }
        public ICardRepository CardRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
            UserRepository = new UserRepository(_dataContext);
            TopicRepository = new TopicRepository(_dataContext);
            CardRepository = new CardRepository(_dataContext);
            RoleRepository = new RoleRepository(_dataContext);

        }

        public async Task CompleteAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
