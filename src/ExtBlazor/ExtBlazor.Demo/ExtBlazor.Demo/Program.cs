using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Components;
using ExtBlazor.Demo.Database;
using ExtBlazor.Demo.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseSwagger();
app.UseSwaggerUI();

AddEndPoints(app);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ExtBlazor.Demo.Client._Imports).Assembly);

await UpdateDatabaseSchema(app);

app.Run();

static async Task UpdateDatabaseSchema(WebApplication app)
{
    var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetService<ExDbContext>();
    if (db != null)
    {
        await db.Database.MigrateAsync();
        Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
    }
}

static void AddEndPoints(WebApplication app)
{
    app.MapGet("/api/users", async ([AsParameters] GetUsersQuery query, IUserService users, CancellationToken ct) => await users.GetUsers(query, ct));
    app.MapGet("/api/usersdtos", async ([AsParameters] GetUserDtosQuery query, IUserService users, CancellationToken ct) => await users.GetUserDtos(query, ct));
    app.MapGet("/api/users/{id:int}", async (int id, IUserService users, CancellationToken ct) => await users.GetUser(id, ct));
}