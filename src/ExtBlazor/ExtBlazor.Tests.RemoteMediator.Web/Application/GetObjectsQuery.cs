using ExtBlazor.RemoteMediator;
using MediatR;

namespace ExtBlazor.Tests.RemoteMediator.Web.Application;

public record GetObjectsQuery : IRequest<IEnumerable<ObjectDto>>, IRemoteRequest<IEnumerable<ObjectDto>>;

public record ObjectDto(int Id, string Name);

public class GetObjectsQueryHandler : IRequestHandler<GetObjectsQuery, IEnumerable<ObjectDto>>
{
    public static readonly List<ObjectDto> Objects = [];
    public Task<IEnumerable<ObjectDto>> Handle(GetObjectsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<ObjectDto>>(Objects);
    }
}
