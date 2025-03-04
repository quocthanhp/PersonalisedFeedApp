using Domain.DTOs.Models.News;

namespace Application.Interfaces
{
    public interface IFetchNewsService
    {
        Task<NewsAPIResponse> FetchNewsAsync(String topic);
    }
}