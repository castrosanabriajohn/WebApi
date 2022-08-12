using System.Net;
using System.Text.Json;

namespace API.Middleware;
public class ErrorHandlingMiddleware
{
  private readonly RequestDelegate _next;
  public ErrorHandlingMiddleware(RequestDelegate next)
  {
    _next = next;
  }
  public async Task Invoke(HttpContext context)
  {
    /* When the next middleware is called on the try, and eventually invoke the endpoints, 
      if an exception is thrown it will be catched and handled by the next method.*/
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }
  private static Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    var code = HttpStatusCode.InternalServerError; // 500 if unexpected
    var result = JsonSerializer.Serialize(
      new { error = "An unexpected error occurred while processing your request." }
    );
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)code;
    return context.Response.WriteAsync(result);
  }
}