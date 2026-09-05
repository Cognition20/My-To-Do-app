using To_Do;
using To_Do.DataAccess;
using To_Do.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation(builder.Configuration)
    .AddServices(builder.Configuration)
    .AddDataAccess(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
