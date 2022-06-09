using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public ITopicRepository TopicRepository { get; }
        public ICardRepository CardRepository { get; }
        public Task CompleteAsync();
    }
}
