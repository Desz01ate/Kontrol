using Kontrol.Abstractions;
using Kontrol.Hubs;
using Kontrol.Implementations;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddLogging(opt => opt.AddConsole());
services.AddSignalR(opt => opt.EnableDetailedErrors = true);

// TODO: register appropriate media server instance according to the host OS.
services.TryAddSingleton<IMediaServer, WindowsMediaServer>();

var app = builder.Build();

app.MapHub<MediaControlHub>("media");

await app.RunAsync();