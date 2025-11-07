using System.Net;
using Gimnasio.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gimnasio.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BussinesException businessException)
            {
                var validation = new 
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = businessException.Message
                };

                var json = new 
                {
                    errors = new[] { validation }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var validation = new 
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = context.Exception.Message
                };

                var json = new 
                {
                    errors = new[] { validation }
                };

                context.Result = new ObjectResult(json)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            
            context.ExceptionHandled = true;
        }
    }
}