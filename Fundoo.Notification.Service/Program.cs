using Fundoo.Notification.Service;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<EmailService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();