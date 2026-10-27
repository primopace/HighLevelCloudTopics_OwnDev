using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Abstraction
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string exchange, string routingKey, T message) where T : class;
    }
}
