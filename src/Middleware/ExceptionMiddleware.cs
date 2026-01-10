using Sho8lana.API.Exceptions;
using Sho8lana.Entities.Models;
using System.Text.Json;

namespace Sho8lana.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            Response response;
            switch (ex)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    string message = ex.Message == new UnauthorizedAccessException().Message ? "Unauthorized" : ex.Message;
                    response = new Response(StatusCodes.Status401Unauthorized, [message]);
                    break;
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = new Response(StatusCodes.Status404NotFound, ex.Message);
                    break;
                case BadRequestException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new Response(StatusCodes.Status400BadRequest, ex.Message);
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = new Response(StatusCodes.Status501NotImplemented, "Something went wrong");
                    break;
            }
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

    }
}
