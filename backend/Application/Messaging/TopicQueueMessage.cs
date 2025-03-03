namespace Application.Messaging
{
    public class TopicQueueMessage
    {
        public string Topic { get; set; } = null!;
        public int UserId { get; set; }
    }
}