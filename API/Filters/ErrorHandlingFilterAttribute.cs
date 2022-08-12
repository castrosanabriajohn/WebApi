using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters;
// This filter will be applied to all of the controllers when an exception is thrown unhandled, OnException() will be invoked
public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
{
  public override void OnException(ExceptionContext context)
  {
    // Getting the exception
    var exception = context.Exception;
    // Creating the exception details, ProblemDetails is an standard error response
    var problemDetails = new ProblemDetails
    {
      Title = "An error occurred while processing your request.",
      Status = (int)HttpStatusCode.InternalServerError,
    };
    // Defining what will be returned to the client 
    /* var errorResult = new { error = "An error occurred while processing your request." }; */
    context.Result = new ObjectResult(problemDetails); // ObjectResult gets the details from problemDetails
    /*     {
          StatusCode = 500
        }; */
    context.ExceptionHandled = true;
  }
}