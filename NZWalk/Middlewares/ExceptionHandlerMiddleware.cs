using System.Net;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Call the next middleware in the pipeline
            await next(context);
        }
        catch (Exception ex)
        {
            var errorId = Guid.NewGuid();

            // Log the exception
            logger.LogError(ex, $"{errorId} : {ex.Message}");

            // Return a custom error response
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var error = new
            {
                Id = errorId,
                ErrorMessage = "Something went wrong! We are looking into it."
            };

            /*var errorResponseJson = JsonSerializer.Serialize(errorResponse);*/

            var errorResponseJson = JsonSerializer.Serialize(error);

            await context.Response.WriteAsync(errorResponseJson);
        }
    }
}
