using System.Threading.Tasks;
using flash_card.business.Services;
using flash_card.data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace flash_card.api.Controllers
{
    [ApiController]
    [Route("/cards")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [AllowAnonymous]
        [HttpGet("/get-all-card")]
        public async Task<ActionResult> GetAll()
        {
            var topics = await _cardService.GetAll();
            return Ok(topics);
        }

        [AllowAnonymous]
        [HttpGet("/get-card-by-id")]
        public async Task<ActionResult> GetById(int id)
        {
            var topic = await _cardService.GetCard(id);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpPost("/create-card")]
        public async Task<ActionResult> Create(FlashCard request)
        {
            var topic = await _cardService.CreateCard(request);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpPut("/update-card")]
        public async Task<ActionResult> Update(FlashCard request)
        {
            var topic = await _cardService.UpdateCard(request);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpDelete("/delete-card")]
        public async Task<ActionResult> Delete(int id)
        {
            var topic = await _cardService.DeleteCard(id);
            return Ok(topic);
        }
    }
}
