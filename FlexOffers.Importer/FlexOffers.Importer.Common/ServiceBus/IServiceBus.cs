
using System;
using System.Runtime.CompilerServices;

namespace FlexOffers.Importer.Common.ServiceBus
{
    /// <summary>
    /// Encapsulate the contract for a service bus, an abstraction in top of MassTransit service bus.
    /// </summary>
    public interface IServiceBus:IDisposable
    {
        void Publish<T>(T message) where T : class;
        void Start(string connectionString);
        void DeclarePublisher<T>(string queue, string exchange, string routingKey);
        void Subscribe(string queueName, Action<IHandlerRegistration> addHandlers);
        void Resolve();
    }
}
