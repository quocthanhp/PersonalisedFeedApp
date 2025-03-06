using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;
using Application.Messaging;

[Route("api/[controller]")]
[ApiController]
public class PreferenceController : ControllerBase
{
    private readonly IPreferenceRepository _repo;
    private readonly IMessagePublisher _publisher;

    public PreferenceController(IPreferenceRepository repo, IMessagePublisher publisher)
    {
        _repo = repo;
        _publisher = publisher;
        _publisher.DeclareQueues(new List<string> { "news-queue", "reddit-queue" });
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Preference>))]
    public async Task<IEnumerable<Preference>> GetPreferences()
    {
        return await _repo.RetrieveAllAsync();
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Preference))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] Preference p)
    {
        if (p is null)
        {
            return BadRequest();
        }

        // Might consider checking dup

        Preference? addPreference = await _repo.CreateAsync(p);
        if (addPreference is null)
        {
            return BadRequest("Repository failed to create preference");
        }
        else
        {
            // Send topic to queue
            var message = new TopicQueueMessage { Topic = addPreference.Topic };// //UserId = addPreference.UserId };
            await _publisher.PublishAsync(message);

            return Ok(new { message = "Preference added and fetching started." });
        }
    }

    
}
