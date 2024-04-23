using FluentValidation;
using Newtonsoft.Json;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred.");
            HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            HandleException(context, ex);
        }
    }

    private void HandleValidationException(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.Headers.Add("Content-Type", "application/json");

        var errors = ex.Errors.Select(error =>
            new { Field = error.PropertyName, Message = error.ErrorMessage });

        var response = new { errors };

        var jsonResponse = JsonConvert.SerializeObject(response);
        context.Response.WriteAsync(jsonResponse);
    }

    private void HandleException(HttpContext context, Exception ex)
    {
        // Handle other types of exceptions here, such as logging and returning a generic error response.
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.Headers.Add("Content-Type", "application/problem+json");
        context.Response.WriteAsync("{\"error\": \"An unexpected error occurred.\"}");
    }
}
