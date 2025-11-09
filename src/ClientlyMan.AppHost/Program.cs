using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("clientlyman-postgres")
    .WithImageTag("16")
    .WithDataVolume();

var database = postgres.AddDatabase("clientlyman", configure: db =>
{
    db.WithUserName("postgres");
    db.WithPassword("postgres");
});

var api = builder.AddProject<Projects.ClientlyMan_Api>("clientlyman-api")
    .WithReference(database)
    .WithEndpoint("http")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

api.WithEnvironment("ConnectionStrings__ClientlyMan", database.Resource.ConnectionStringExpression);

builder.Build().Run();
