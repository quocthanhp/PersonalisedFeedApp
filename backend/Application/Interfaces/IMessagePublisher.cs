using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message);
        void DeclareQueues(IEnumerable<string> queueNames);
    }
}