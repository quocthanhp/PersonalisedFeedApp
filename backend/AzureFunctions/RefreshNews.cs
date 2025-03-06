// using System;
// using System.Threading.Tasks;
// using Application.Interfaces;
// using Microsoft.Azure.WebJobs;
// using Microsoft.Extensions.Logging;
// using Application.Messaging;
// using System.Text.Json;

// namespace AzureFunctions
// {
//     public class RefreshNews
//     {
//         private readonly IPreferenceRepository _preferenceRepository;
//         private readonly IMessagePublisher _publisher;
//         public RefreshNews(IPreferenceRepository preferenceRepository, IMessagePublisher publisher)
//         {
//             _preferenceRepository = preferenceRepository;
//             _publisher = publisher;
//         }

//         [FunctionName("RefreshNews")]
//         public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer, ILogger log)
//         {
//             log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

//             var preferences = await _preferenceRepository.RetrieveAllAsync();

//             foreach (var preference in preferences)
//             {
//                 var message = new TopicQueueMessage
//                 {
//                     Topic = preference.Topic
//                 };

//                 await _publisher.PublishAsync("new-topic", message);
//             }
//         }

//     }
// }