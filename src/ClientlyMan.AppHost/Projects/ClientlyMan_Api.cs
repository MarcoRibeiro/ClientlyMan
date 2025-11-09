using Aspire.Hosting;

namespace Projects;

/// <summary>
/// Metadata linking the AppHost to the API project.
/// </summary>
public class ClientlyMan_Api : IProjectMetadata
{
    /// <inheritdoc />
    public string ProjectPath => "../ClientlyMan.Api/ClientlyMan.Api.csproj";
}
