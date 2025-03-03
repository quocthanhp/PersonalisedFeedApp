using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentItemController : ControllerBase
{
    private readonly IContentItemRepository _repo;

    public ContentItemController(IContentItemRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ContentItem>))]
    public async Task<IEnumerable<ContentItem>> GetContentItems(string userId)
    {
        return await _repo.RetrieveAllAsync(userId);
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(ContentItem))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ContentItem c)
    {
        if (c is null)
        {
            return BadRequest();
        }

        ContentItem? addedContentItem = await _repo.CreateAsync(c);
        if (addedContentItem is null)
        {
            return BadRequest("Repository failed to create content item");
        }
        else
        {
            return Ok(new { message = "Content Item added." });
        }
    }
}