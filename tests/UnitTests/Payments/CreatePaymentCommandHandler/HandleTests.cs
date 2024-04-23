using FluentAssertions;
using NSubstitute;
using WebApiTemplate.Application.Payments.Commands;
using WebApiTemplate.Core.Enum;
using WebApiTemplate.Core.Models.Transaction;
using WebApiTemplate.Infrastructure.Caching;
using Xunit;

namespace WebApiTemplate.UnitTests.Customers.CreatePaymentCommandHandler;

public class HandleTests
{
    private readonly CreatePaymentCommandValidator _validator = new();

    [Fact]
    public async Task PaymentGateway_ShouldReturnApproved()
    {
        // Arrange
        var mockCache = Substitute.For<ICacheService<string, TransactionResult>>();
        var sut = new Application.Payments.Commands.CreatePaymentCommandHandler(mockCache);

        var paymentDetails = new CreatePaymentCommand
        {
            CardNumber = "4242424242424242", // A valid credit card number, typically used in testing
            ExpiryMonth = DateTime.Now.Month, // Current month or any valid future month (1-12)
            ExpiryYear =
                DateTime.Now.Year +
                1, // A valid future year, ensure it's within the range you expect (e.g., not past the current year + 20)
            CVV = "123", // A valid CVV, usually a 3-digit number (or 4 digits for some card types)
            Amount = 100.00m, // A positive amount greater than zero
            Currency = "USD", // A valid 3-letter ISO currency code
            MerchantId = "1" // A valid merchant ID, as per your application's requirements
        };


        // Act
        var result = await sut.Handle(paymentDetails);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Message.Should().Contain("approved");
    }

    [Fact]
    public async Task PaymentGateway_ShouldReturnDecline()
    {
        // Arrange
        var mockCache = Substitute.For<ICacheService<string, TransactionResult>>();
        var sut = new Application.Payments.Commands.CreatePaymentCommandHandler(mockCache);

        var paymentDetails = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111", // A valid credit card number, typically used in testing
            ExpiryMonth = DateTime.Now.Month, // Current month or any valid future month (1-12)
            ExpiryYear =
                DateTime.Now.Year +
                1, // A valid future year, ensure it's within the range you expect (e.g., not past the current year + 20)
            CVV = "123", // A valid CVV, usually a 3-digit number (or 4 digits for some card types)
            Amount = 100.00m, // A positive amount greater than zero
            Currency = "USD", // A valid 3-letter ISO currency code
            MerchantId = "0" // A valid merchant ID, as per your application's requirements
        };


        // Act
        var result = await sut.Handle(paymentDetails);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Message.Should().Contain("denied");
    }


    [Fact]
    public void Validator_ShouldRejectInvalidCardNumber()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "1234567890", // Clearly invalid card number
            ExpiryMonth = DateTime.Now.Month,
            ExpiryYear = DateTime.Now.Year,
            CVV = "123",
            Amount = 100.00m,
            Currency = "USD",
            MerchantId = "1"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validator_ShouldRejectInvalidExpiryMonth()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111",
            ExpiryMonth = 13, // Invalid month
            ExpiryYear = DateTime.Now.Year,
            CVV = "123",
            Amount = 100.00m,
            Currency = "USD",
            MerchantId = "1"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "ExpiryMonth");
    }

    [Fact]
    public void Validator_ShouldRejectInvalidExpiryYear()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111",
            ExpiryMonth = DateTime.Now.Month,
            ExpiryYear = DateTime.Now.Year - 1, // Past year
            CVV = "123",
            Amount = 100.00m,
            Currency = "USD",
            MerchantId = "0"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "ExpiryYear");
    }

    [Fact]
    public void Validator_ShouldRejectInvalidCVV()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111",
            ExpiryMonth = DateTime.Now.Month,
            ExpiryYear = DateTime.Now.Year,
            CVV = "abc", // Non-numeric CVV
            Amount = 100.00m,
            Currency = "USD",
            MerchantId = "0"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "CVV");
    }

    [Fact]
    public void Validator_ShouldRejectInvalidAmount()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111",
            ExpiryMonth = DateTime.Now.Month,
            ExpiryYear = DateTime.Now.Year,
            CVV = "123",
            Amount = 0, // Amount must be greater than zero
            Currency = "USD",
            MerchantId = "1"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Amount");
    }

    [Fact]
    public void Validator_ShouldRejectInvalidCurrency()
    {
        // Arrange
        var command = new CreatePaymentCommand
        {
            CardNumber = "4111111111111111",
            ExpiryMonth = DateTime.Now.Month,
            ExpiryYear = DateTime.Now.Year,
            CVV = "123",
            Amount = 100.00m,
            Currency = "US", // Invalid ISO currency code
            MerchantId = "1"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Currency");
    }
}
