using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ocelot configuration file add karo
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Fundoo API Gateway is Running!");

// Ocelot middleware use karo
await app.UseOcelot();

app.Run();