using flash_card.data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flash_card.business.Services
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAll();
        Task<Topic> GetTopic(int id);
        Task<Topic> CreateTopic(Topic topic);
        Task<Topic> UpdateTopic(Topic topic);
        Task<string> DeleteTopic(int id);
    }
}
