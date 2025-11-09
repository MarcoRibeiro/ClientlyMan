using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Application.Customers.Services;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ClientlyMan.Tests.Unit.Customers;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repository = new();
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomerServiceTests()
    {
        _createValidator = MockValidator<CreateCustomerDto>();
        _updateValidator = MockValidator<UpdateCustomerDto>();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedCustomer_WhenInputIsValid()
    {
        // Arrange
        var dto = new CreateCustomerDto("Jane Doe", "123", "jane@contoso.com", null, null);
        var service = new CustomerService(_repository.Object, _createValidator, _updateValidator);
        _repository.Setup(x => x.ExistsWithTaxNumberAsync(dto.TaxNumber, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repository.Setup(x => x.AddAsync(It.IsAny<ClientlyMan.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClientlyMan.Domain.Entities.Customer customer, CancellationToken _) => customer);

        // Act
        var result = await service.CreateAsync(dto, CancellationToken.None);

        // Assert
        result.Name.Should().Be(dto.Name);
        _repository.Verify(x => x.AddAsync(It.IsAny<ClientlyMan.Domain.Entities.Customer>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowValidationException_WhenTaxNumberConflicts()
    {
        // Arrange
        var dto = new UpdateCustomerDto("Jane Doe", "123", "jane@contoso.com", null, null);
        var service = new CustomerService(_repository.Object, _createValidator, _updateValidator);
        var existing = new ClientlyMan.Domain.Entities.Customer
        {
            Id = Guid.NewGuid(),
            Name = "Jane",
            TaxNumber = "111",
            Email = "old@contoso.com",
            CreatedAt = DateTime.UtcNow
        };

        _repository.Setup(x => x.GetByIdAsync(existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        _repository.Setup(x => x.ExistsWithTaxNumberAsync(dto.TaxNumber, existing.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var action = () => service.UpdateAsync(existing.Id, dto, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ValidationException>();
    }

    private static IValidator<T> MockValidator<T>()
    {
        var mock = new Mock<IValidator<T>>();
        mock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<T>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        mock.Setup(x => x.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        mock.Setup(x => x.Validate(It.IsAny<T>()))
            .Returns(new ValidationResult());
        mock.Setup(x => x.Validate(It.IsAny<ValidationContext<T>>()))
            .Returns(new ValidationResult());
        return mock.Object;
    }
}
