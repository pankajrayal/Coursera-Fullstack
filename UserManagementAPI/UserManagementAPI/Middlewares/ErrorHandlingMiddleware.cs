using System.Net;
using System.Text.Json;

namespace UserManagementAPI.Middlewares {
  public class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
      try {
        // Proceed to the next middleware
        await _next(context);
      } catch(Exception ex) {
        // Handle the exception and return a consistent JSON error response
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
      // Log the exception (optional, can be replaced with a logging framework)
      Console.WriteLine($"Unhandled Exception: {exception.Message}");

      // Set the response status code and content type
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
      context.Response.ContentType = "application/json";

      // Create the error response
      var errorResponse = new { error = "Internal server error." };

      // Serialize the error response to JSON and write it to the response body
      var errorJson = JsonSerializer.Serialize(errorResponse);
      return context.Response.WriteAsync(errorJson);
    }
  }
}
