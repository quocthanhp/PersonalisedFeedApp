using Infrastructure.External.Models;

namespace Application.Interfaces
{
    public interface IFetchNewsService
    {
        Task<NewsAPIResponse> FetchNewsAsync(String topic);
    }
}