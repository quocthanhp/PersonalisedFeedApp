
using Domain.DTOs.Models.Reddit;

namespace Application.Interfaces
{
    public interface IFetchRedditPostService
    {
        Task<RedditPostAPIResponse> FetchRedditPostAsync(String topic);
    }
}