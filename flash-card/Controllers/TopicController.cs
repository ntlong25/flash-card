using System.Threading.Tasks;
using flash_card.business.Services;
using flash_card.data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace flash_card.api.Controllers
{
    [ApiController]
    [Route("/topics")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [AllowAnonymous]
        [HttpGet("/get-all-topic")]
        public async Task<ActionResult> GetAll()
        {
            var topics = await _topicService.GetAll();
            return Ok(topics);
        }

        [AllowAnonymous]
        [HttpGet("/get-topic-by-id")]
        public async Task<ActionResult> GetById(int id)
        {
            var topic = await _topicService.GetTopic(id);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpPost("/create-topic")]
        public async Task<ActionResult> Create(Topic request)
        {
            var topic = await _topicService.CreateTopic(request);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpPut("/update-topic")]
        public async Task<ActionResult> Update(Topic request)
        {
            var topic = await _topicService.UpdateTopic(request);
            return Ok(topic);
        }

        [AllowAnonymous]
        [HttpDelete("/delete-topic")]
        public async Task<ActionResult> Delete(int id)
        {
            var topic = await _topicService.DeleteTopic(id);
            return Ok(topic);
        }
    }
}
