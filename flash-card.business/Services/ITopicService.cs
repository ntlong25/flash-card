using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Services
{
    public interface ITopicService
    {
        Task<Topic> List();
        Task<Topic> GetTopic(int id);
        Task<Topic> CreateTopic(Topic topic);
    }
}
