using ExtBlazor.RemoteMediator;
using MediatR;

namespace ExtBlazor.Tests.RemoteMediator.Web.Application;

public record CreateObjectCommand(int Id, string Name) : IRequest, IRemoteRequest;
public class CreateObjectCommandHandler : IRequestHandler<CreateObjectCommand>
{
    public Task Handle(CreateObjectCommand command, CancellationToken cancellationToken)
    {
        GetObjectsQueryHandler.Objects.Add(new(command.Id, command.Name));
        return Task.CompletedTask;
    }
}