using FictionalCompany.Entities.Database;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FCDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
