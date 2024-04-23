//using System.Net;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using WebApiTemplate.Api.Customers.Requests;
//using WebApiTemplate.Application.Payments.Commands;
//using WebApiTemplate.Infrastructure.Persistence;
//using Xunit;

//namespace WebApiTemplate.IntegrationTests.Customers.Controllers;

//public class CreateTests : BaseTestClass
//{
//    public CreateTests(AppWebApplicationFactory factory)
//        : base(factory) { }

//    [Fact]
//    public async Task WithValidRequestShouldCreateCorrectly()
//    {
//        //await _factory.ResetDatabase();

//        var client = _factory.CreateClient();
//        var request = new CreatePaymentCommand();
//        var response = await client.PostAsJsonAsync($"/api/payments", request);

//        response.StatusCode.Should().Be(HttpStatusCode.Created);

//        var result = await context.Customers.SingleOrDefaultAsync();
//        result.Should().NotBeNull();
//    }
//}



