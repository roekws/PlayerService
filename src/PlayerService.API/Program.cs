using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using PlayerService.Validation;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); ;

var connection = builder.Configuration.GetConnectionString("Database");

if (string.IsNullOrEmpty(connection))
{
  Console.WriteLine("Default connectnion not found");
  return;
}

builder.Services.AddDbContext<PlayerContext>(options => options.UseNpgsql(connection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();
