using flash_card.business.Repository;
using flash_card.data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace flash_card.business.Services.Implement
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public TopicService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public Task<Topic> CreateTopic(Topic topic)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> GetTopic(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Topic> List()
        {
            throw new NotImplementedException();
        }
    }
}
