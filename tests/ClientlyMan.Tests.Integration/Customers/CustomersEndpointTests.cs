using System.Net;
using System.Net.Http.Json;
using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Infrastructure.Persistence;
using ClientlyMan.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientlyMan.Tests.Integration.Customers;

public class CustomersEndpointTests : IClassFixture<ClientlyManApiFactory>
{
    private readonly ClientlyManApiFactory _factory;

    public CustomersEndpointTests(ClientlyManApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnOk()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/customers");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostCustomers_ShouldCreateCustomer()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClientlyManDbContext>();
        db.Customers.RemoveRange(db.Customers);
        await db.SaveChangesAsync();

        using var client = _factory.CreateClient();
        var payload = new CreateCustomerDto("Integration Tester", "IT123", "tester@clientlyman.dev", null, null);

        // Act
        var response = await client.PostAsJsonAsync("/api/customers", payload);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var customer = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        customer.Should().NotBeNull();
        customer!.Name.Should().Be(payload.Name);
    }
}
