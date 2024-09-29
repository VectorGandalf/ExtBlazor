using ExtBlazor.Core;

namespace ExtBlazor.Demo.Client.Services;

public interface ICommandService
{
    Task Execute(ICommand command);
    Task<TResult> Execute<TResult>(ICommand<TResult> command);
}
