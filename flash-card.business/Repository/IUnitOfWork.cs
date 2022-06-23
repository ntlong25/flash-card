using flash_card.business.Repository.Implement;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Repository
{
    public interface IUnitOfWork
    {
        public IRepository<User> UserRepository { get; }
        public IRepository<Role> RoleRepository { get; }
        public IRepository<Topic> TopicRepository { get; }
        public IRepository<FlashCard> CardRepository { get; }
        public Task CompleteAsync();
        void Dispose();
    }
}
