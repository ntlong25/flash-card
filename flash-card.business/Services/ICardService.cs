using flash_card.data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flash_card.business.Services
{
    public interface ICardService
    {
        Task<List<FlashCard>> GetAll();
        Task<FlashCard> GetCard(int id);
        Task<FlashCard> CreateCard(FlashCard card);
        Task<FlashCard> UpdateCard(FlashCard request);
        Task<string> DeleteCard(int id);
    }
}
