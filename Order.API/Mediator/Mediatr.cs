namespace Order.API.Mediator
{
    public class Mediatr : IMediatr
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediatr(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResult> Send<TResult>(ICommand<TResult> command)
        {
            var handlerType = typeof(ICommandHandler<,>)
                .MakeGenericType(command.GetType(), typeof(TResult));

            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new Exception($"Handler not found for {command.GetType().Name}");

            return await handler.Handle((dynamic)command);
        }

        public async Task<TResult> Query<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>)
                .MakeGenericType(query.GetType(), typeof(TResult));

            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new Exception($"Handler not found for {query.GetType().Name}");

            return await handler.Handle((dynamic)query);
        }
    }
}
