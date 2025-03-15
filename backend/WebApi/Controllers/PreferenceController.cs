using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;

using System.Runtime.InteropServices;
using Application.Extensions;
using Application.Messaging;
using Domain.DTOs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


[Route("api/[controller]")]
[ApiController]
public class PreferenceController : ControllerBase
{
    private readonly IPreferenceRepository _repo;
    private readonly IMessagePublisher _publisher;

    private readonly UserManager<User> _userManager;

    public PreferenceController(IPreferenceRepository repo, IMessagePublisher publisher, UserManager<User> userManager)
    {
        _repo = repo;
        _publisher = publisher;
        _userManager = userManager;
        _publisher.DeclareQueues(new List<string> { "news-queue", "reddit-queue" });
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Preference>))]
    public async Task<IEnumerable<Preference>> GetPreferences()
    {
        return await _repo.RetrieveAllAsync();
    }


    [HttpPost]
    [Authorize]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] NewPreference p)
    {
        try
        {
            Console.WriteLine("Create preference");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        var userName = User.GetUserName();
        if (userName is null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return Unauthorized(); // Ensure the user is logged in
        }

        if (p is null)
        {
            return BadRequest();
        }

        var preference = new Preference
        {
            UserId = user.Id,
            Topic = p.Topic
        };

        // Might consider checking dup

        Preference? addPreference = await _repo.CreateAsync(preference);
        if (addPreference is null)
        {
            return BadRequest("Repository failed to create preference");
        }
        else
        {
            // Send topic to queue
            try
            {
                var message = new TopicQueueMessage { Topic = addPreference.Topic };// //UserId = addPreference.UserId };
                await _publisher.PublishAsync(message);
            }
            catch (Exception e)
            {
                return BadRequest("Failed to send message to queue");
            }

            return Ok(new { message = "Preference added and fetching started." });
        }
    }
}

