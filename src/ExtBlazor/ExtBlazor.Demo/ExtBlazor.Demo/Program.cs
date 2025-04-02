using ExtBlazor.Demo;
using ExtBlazor.Demo.Components;
using ExtBlazor.Demo.Database;
using ExtBlazor.Demo.Services;
using ExtBlazor.Events.SignalR.Client;
using ExtBlazor.Events.SignalR.Server;
using ExtBlazor.RemoteMediator.Server;
using ExtBlazor.Stash;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var hubBuilderConfiguration = new EventsHubConnectionBuilder();

builder.Services.AddHostedService<TickerHostedService>();

builder.Services
    .AddSignalREventService(new() { EventRouter = EventsConfiguration.EventRouter })
    .AddHubOptions<EventHub>(EventsConfiguration.HubOptions);

builder.Services.AddSignalREventServiceClient();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddDbContext<ExDbContext>(_ => _.UseSqlite(@"Data Source=.\ex.db"))
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddScoped<IUserService, UserService>();

builder.Services
    .AddControllers();

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(typeof(UserService).Assembly);
});

builder.Services.AddRemoteMediatorServer(config =>
{
    config.MediatorCallback = async (IBaseRequest request, IMediator mediator, CancellationToken ct) => await mediator.Send(request, ct);
});

builder.Services.AddStashService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSignalREventService();
app.MapRemoteMediatorEndPoint();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ExtBlazor.Demo.Client._Imports).Assembly);

await UpdateDatabaseSchema(app);

app.Run();

static async Task UpdateDatabaseSchema(WebApplication app)
{
    using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetService<ExDbContext>();
    if (db != null)
    {
        await db.Database.MigrateAsync();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
    }
}