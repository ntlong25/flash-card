using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using flash_card.business.Repository;
using flash_card.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace flash_card.business.Services.Implement
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FlashCard>> GetAll()
        {
            try
            {
                var items = await _unitOfWork.CardRepository
                    .FindAsync(t => t.Status == true)
                    .Include(t => t.Topic)
                    .ToListAsync();
                return items;
            }
            catch (Exception ex)
            {
                return Result<List<FlashCard>>.Error(new[] { ex.Message });
            }
        }

        public async Task<FlashCard> GetCard(int id)
        {
            try
            {
                var item = await _unitOfWork.CardRepository
                    .FindAsync(c => c.Status == true && c.Id == id)
                    .Include(t => t.Topic)
                    .FirstOrDefaultAsync();
                return item;
            }
            catch (Exception ex)
            {
                return Result<FlashCard>.Error(new[] { ex.Message });
            }
        }

        public async Task<FlashCard> CreateCard(FlashCard card)
        {
            FlashCard result;
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
                var item = card;
                result = await _unitOfWork.CardRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                // TODO: Log details here
                return Result<FlashCard>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();

            return result;
        }

        public async Task<FlashCard> UpdateCard(FlashCard request)
        {
            var cardExist = await _unitOfWork.CardRepository.FindAsync(r => r.Id == request.Id).FirstOrDefaultAsync();
            if (cardExist == null) return Result<FlashCard>.NotFound();
            try
            {
                cardExist.Question = request.Question;
                cardExist.Answer = request.Answer;
                cardExist.ImgQuestion = request.ImgQuestion;
                cardExist.ImgAnswer = request.ImgAnswer;
                await _unitOfWork.CardRepository.UpdateAsync(cardExist);
            }
            catch (Exception ex)
            {
                return Result<FlashCard>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();

            return cardExist;
        }

        public async Task<string> DeleteCard(int id)
        {
            var cardExist = await _unitOfWork.CardRepository.FindAsync(r => r.Id == id).FirstOrDefaultAsync();
            if (cardExist == null) return Result<string>.Error("Card not found");
            try
            {
                cardExist.Status = false;
                await _unitOfWork.CardRepository.UpdateAsync(cardExist);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(new[] { ex.Message });
            }
            await _unitOfWork.CompleteAsync();
            _unitOfWork.Dispose();

            return Result<string>.Success("The flash card has been deleted.");
        }
    }
}