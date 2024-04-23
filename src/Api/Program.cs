using FluentValidation;
using HumbleMediator;
using Serilog;
using Serilog.Events;
using SimpleInjector;
using WebApiTemplate.Application.Common.Logging;
using WebApiTemplate.Application.Common.Validation;
using WebApiTemplate.Application.Payments.Commands;
using WebApiTemplate.Application.Payments.Queries;
using WebApiTemplate.Infrastructure.Caching;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(); // replace built-in logging with Serilog

    // Add services to the container.
    builder.Services.AddControllers();

    // swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();
    //builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSingleton(typeof(ICacheService<,>), typeof(InMemoryCacheService<,>));
    builder.Services.AddSingleton<GlobalExceptionHandlerMiddleware>();
    //app.Services.AddMemoryCache();


    // SimpleInjector
    var container = WebApiTemplate.Api.Program.Container;
    container.Options.DefaultLifestyle = Lifestyle.Singleton;
    builder.Services.AddSimpleInjector(
        container,
        options => options.AddAspNetCore().AddControllerActivation()
    );

    container.Collection.Register(
        typeof(IValidator<>),
        typeof(GetPaymentByIdQueryValidator).Assembly
    );

    // mediator
    container.Register<IMediator>(() => new Mediator(container.GetInstance));
    // mediator handlers
    container.Register(typeof(ICommandHandler<,>), typeof(CreatePaymentCommandHandler).Assembly);
    container.Register(typeof(IQueryHandler<,>), typeof(GetPaymentByIdQueryHandler).Assembly);

    // mediator handlers decorators - queries pipeline
    container.RegisterDecorator(
        typeof(IQueryHandler<,>),
        typeof(QueryHandlerValidationDecorator<,>)
    );


    container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(QueryHandlerLoggingDecorator<,>));

    // mediator handlers decorators - commands pipeline
    container.RegisterDecorator(
        typeof(ICommandHandler<,>),
        typeof(CommandHandlerValidationDecorator<,>)
    );
    container.RegisterDecorator(
        typeof(ICommandHandler<,>),
        typeof(CommandHandlerLoggingDecorator<,>)
    );

    var app = builder.Build();

    app.Services.UseSimpleInjector(container);

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.

    //Enable Swagger
    app.UseSwagger();
    app.UseSwaggerUI();


    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    app.UseAuthorization();
    app.MapControllers();


    container.Verify();

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

namespace WebApiTemplate.Api
{
    public class Program
    {
        public static readonly Container Container = new();
    }
}
