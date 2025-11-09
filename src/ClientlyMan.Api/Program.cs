using ClientlyMan.Application.Common;
using ClientlyMan.Infrastructure;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new() { Title = "ClientlyMan API", Version = "v1" });
    });

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

/// <summary>
/// Exposed for integration testing purposes.
/// </summary>
public partial class Program
{
}
