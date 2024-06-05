using ExtBlazor.Core;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Demo.Components;
using ExtBlazor.Demo.Database;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddDbContext<ExDbContext>(o =>
    {
        o.UseSqlite("Data Source=" + GeExtDbPath());
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapGet("/api/users", async ([AsParameters] GetUsersQuery query, ExDbContext context) =>
{
    var q = context.Users.AsQueryable();
    foreach (var token in (query.Search ?? "").Split(' ')) 
    {
        q = q.Where(_ => 
            _.Name.Contains(token) ||
            _.Phone.Contains(token) || 
            _.Email.Contains(token) || 
            _.Username.Contains(token));       
    }

    return await q.PageAsync(query);
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ExtBlazor.Demo.Client._Imports).Assembly);

await UpdateDatabaseSchema(app);

app.Run();

static string GeExtDbPath()
{
    //var folder = Environment.SpecialFolder.AdminTools;
    //var path = Environment.GetFolderPath(folder);
    //var subpath = System.IO.Path.Join(path, "ExtBlazor.Demo");
    //Directory.CreateDirectory(System.IO.Path.Join(path, "ExtBlazor.Demo"));
    //return System.IO.Path.Join(path, "ExtBlazor.Demo", "ex.db");
    return @".\ex.db";
}

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