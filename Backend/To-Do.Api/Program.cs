using To_Do.Api;
using To_Do.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services
    .AddDataAccess(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();
