using flash_card.business.Repository;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace flash_card.business.Services.Implement
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Topic>> GetAll()
        {
            try
            {
                var items = await _unitOfWork.TopicRepository
                    .FindAsync(t => t.Status == true)
                    .Include(u => u.User)
                        .ThenInclude(r => r.Role)
                    .Include(c => c.FlashCards)
                    .ToListAsync();
                return new List<Topic>(items);
            }
            catch(Exception ex)
            {
                // TODO: Log details here
                return Result<List<Topic>>.Error(new[] { ex.Message });
            }
        }

        public async Task<Topic> GetTopic(int id)
        {
            try
            {
                var item = await _unitOfWork.TopicRepository
                    .FindAsync(t => t.Status == true && t.Id == id)
                    .Include(u => u.User)
                        .ThenInclude(r => r.Role)
                    .Include(c => c.FlashCards)
                    .FirstOrDefaultAsync();
                return item;
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<Topic>.Error(new[] { ex.Message });
            }
        }

        public async Task<Topic> CreateTopic(Topic topic)
        {
            Topic result;
            try
            {
                //var item = new Topic
                //{
                //    Name = topic.Name,
                //    Status = true,
                //    UserId = topic.UserId,
                //    User = topic.User,
                //    FlashCards = topic.FlashCards
                //};
                var item = topic;
                result = await _unitOfWork.TopicRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<Topic>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();

            return result;
        }

        public async Task<Topic> UpdateTopic(Topic topic)
        {
            var topicExist = await _unitOfWork.TopicRepository.FindAsync(r => r.Id == topic.Id).FirstOrDefaultAsync();
            if (topicExist == null) return Result<Topic>.NotFound();
            try
            {
                topicExist.Name = topic.Name;
                await _unitOfWork.TopicRepository.UpdateAsync(topicExist);
            }
            catch (Exception ex)
            {
                return Result<Topic>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();
            
            return topicExist;
        }

        public async Task<string> DeleteTopic(int id)
        {
            var topicExist = await _unitOfWork.TopicRepository.FindAsync(r => r.Id == id).FirstOrDefaultAsync();
            if (topicExist == null) return Result<string>.Error("Topic not found");
            try
            {
                topicExist.Status = false;
                await _unitOfWork.TopicRepository.UpdateAsync(topicExist);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();

            return Result<string>.Success("The topic has been deleted.");
        }
    }
}
