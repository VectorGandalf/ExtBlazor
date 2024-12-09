using ExtBlazor.RemoteMediator.Server;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddRemoteMediatorServer(config =>
{
    config.MediatorCallback = async (IBaseRequest request, IMediator mediator) => await mediator.Send(request);
});

var app = builder.Build();
app.MapRemoteMediatorEndPoint();
app.Run();

public partial class Program { }