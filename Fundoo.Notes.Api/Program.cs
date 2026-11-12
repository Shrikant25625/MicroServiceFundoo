using Fundoo.Notes.Application.Interfaces;
using Fundoo.Notes.Application.Services;
using Fundoo.Notes.Infrastructure.Data;
using Fundoo.Notes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register MySQL Database Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Register Redis Distributed Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "FundooNotes_";
});

// 3. Register Clean Architecture Dependencies (Repositories & Services)
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteApplication, NoteApplication>();

// Add controllers & swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();