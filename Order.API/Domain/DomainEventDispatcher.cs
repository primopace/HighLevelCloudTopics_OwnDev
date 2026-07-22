namespace Order.API.Domain
{
    public class DomainEventDispatcher: IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task DispatchAsync(IDomainEvent domainEvent)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);
            foreach(var handler in handlers)
            {
                var method = handlerType.GetMethod("Handle");

                if (method == null)
                    throw new InvalidOperationException(
                        $"Handle method not found on {handlerType.Name}");

                var task = (Task)method.Invoke(handler, new object[] { domainEvent })!;

                await task;

            }
        }

        
    }
}
