namespace Order.API.Mediator
{
    public interface IMediatr
    {
        Task<TResult> Send<TResult>(ICommand<TResult> command);

        Task<TResult> Query<TResult>(IQuery<TResult> query);
    }
}
