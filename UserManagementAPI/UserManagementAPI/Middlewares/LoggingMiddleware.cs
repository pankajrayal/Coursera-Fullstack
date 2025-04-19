namespace UserManagementAPI.Middlewares {
  public class LoggingMiddleware {
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next) {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context) {
      // Log the request
      Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

      // Proceed to the next middleware
      await _next(context);

      // Log the response
      Console.WriteLine($"Outgoing Response: {context.Response.StatusCode}");
    }
  }
}