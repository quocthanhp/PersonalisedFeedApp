using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;


namespace AzureFunctions
{
    public class FetchNews
    {
        private readonly HttpClient client = new();
        private readonly IFetchNewsService _fetchNewsService;

        public FetchNews(IFetchNewsService fetchNewsService)
        {
            _fetchNewsService = fetchNewsService;
        }

        [FunctionName("FetchNews")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "news/fetch")] HttpRequest req)
        {
            string topic = req.Query["topic"];

            if (string.IsNullOrEmpty(topic))
            {
                return new BadRequestObjectResult("Please pass a topic in the query string");
            }

            var newsResponse = await _fetchNewsService.FetchNewsAsync(topic);

            return new OkObjectResult(newsResponse);
        }
    }
}
