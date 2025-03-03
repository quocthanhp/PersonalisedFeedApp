using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Application.Interfaces;
using Application.Messaging;
using System.Text.Json;
using Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class FetchNews
    {
        private readonly IFetchNewsService _fetchNewsService;
        private readonly IContentItemRepository _contentItemRepository;
        public FetchNews(IFetchNewsService fetchNewsService, IContentItemRepository contentItemRepository)
        {
            _fetchNewsService = fetchNewsService;
            _contentItemRepository = contentItemRepository;
        }

        [FunctionName("FetchNews")]
        public async Task Run(
            [RabbitMQTrigger("new-topic", ConnectionStringSetting = "RabbitMQConnection")] byte[] messageBytes
            , ILogger log)
        {
            TopicQueueMessage message = JsonSerializer.Deserialize<TopicQueueMessage>(messageBytes);
            log.LogInformation($"Fetching news for topic: {message.Topic}");

            if (message is null)
            {
                log.LogInformation("null message");
                return;
            }

            var topic = message.Topic;
            var newsArticles = await _fetchNewsService.FetchNewsAsync(topic);

            if (newsArticles is null)
            {
                log.LogInformation("null articles");
                return;
                
            }

            IEnumerable<NewsArticle> articles = newsArticles.Articles.Select(a => new NewsArticle
            {
                Author = a.Author,
                Source = a.Source.Name,
                Content = a.Content,
                Title = a.Title,
                Topic = topic,
                PublishedAt = a?.PublishedAt ?? System.DateTime.Now
            });

            log.LogInformation("start adding articles to DB...");
            //await _contentItemRepository.CreateBulkAsync(articles);
        }
    }
}
