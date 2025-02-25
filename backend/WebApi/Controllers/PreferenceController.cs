using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc; 

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreferenceController : ControllerBase
{
    private readonly IPreferenceRepository _repo;

    public PreferenceController(IPreferenceRepository repo)
    {
        _repo = repo;
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

        Preference? addPreference = await _repo.CreateAsync(p);
        if (addPreference is null)
        {
            return BadRequest("Repository failed to create preference");
        }
        else
        {
            // Trigger Azure function to fetch content

            return Ok(new { message = "Preference added and fetching started." });
        }
    }

    
}
