using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Application.Interfaces;
using Application.Messaging;
using System.Text.Json;
using Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;

namespace AzureFunctions
{
    public class FetchReddit
    {
        private readonly IFetchRedditPostService _fetchRedditService;
        private readonly IContentItemRepository _contentItemRepository;

        public FetchReddit(IFetchRedditPostService fetchRedditService, IContentItemRepository contentItemRepository)
        {
            _fetchRedditService = fetchRedditService;
            _contentItemRepository = contentItemRepository;
        }

        [FunctionName("FetchReddit")]
        public async Task Run(
            [RabbitMQTrigger("reddit-queue", ConnectionStringSetting = "RabbitMQConnection")] byte[] messageBytes
            , ILogger log)
        {
            TopicQueueMessage message = JsonSerializer.Deserialize<TopicQueueMessage>(messageBytes);
            log.LogInformation($"Fetching Reddit posts for topic: {message.Topic}");

            if (message is null)
            {
                log.LogInformation("null message");
                return;
            }

            var topic = message.Topic;
            var redditPosts = await _fetchRedditService.FetchRedditPostAsync(topic);

            if (redditPosts is null || redditPosts.Status == Domain.DTOs.Enums.Status.Error)
            {
                log.LogInformation("null posts or error in fetching posts");
                return;
            }

            IEnumerable<SocialMedia> posts = redditPosts.Data.Children.Select(p => new SocialMedia
            {
                Author = p.Data.Author,
                Source = "Reddit",
                Content = p.Data.Selftext,
                Thumbnail = p.Data.Thumbnail,
                Title = p.Data.Title,
                Topic = topic,
                PublishedAt = DateTimeOffset.FromUnixTimeSeconds(p.Data.CreatedUtc).DateTime,
            });

            log.LogInformation("start adding posts to DB...");

            await _contentItemRepository.CreateBulkAsync(posts);
        }

    }
}