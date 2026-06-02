using LekhaChitra.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LekhaChitra.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
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

                await HandleExceptionAsync(context, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var problemDetails = new ProblemDetails();

            switch (exception)
            {
              
                case NotFoundException notFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = notFoundException.Message;
                    problemDetails.Title = "Not Found";
                    problemDetails.Type = notFoundException.GetType().Name;
                    problemDetails.Instance =
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    break;

                case BadRequestException badRequestException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = badRequestException.Message;
                    problemDetails.Title = "Bad Request";
                    problemDetails.Type = badRequestException.GetType().Name;
                    problemDetails.Instance =
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    break;
                case JwtGenerationFailedException jwtGenerationFailedException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = jwtGenerationFailedException.Message;
                    problemDetails.Title = "Jwt Generation Failed";
                    problemDetails.Type = jwtGenerationFailedException.GetType().Name;
                    problemDetails.Instance =
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    break;
                case ResponseAlreadyStartedException responseAlreadyStartedException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = responseAlreadyStartedException.Message;
                    problemDetails.Title = "Response has already started";
                    problemDetails.Type = responseAlreadyStartedException.GetType().Name;
                    problemDetails.Instance =
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = exception.Message;
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Type = exception.GetType().Name;
                    problemDetails.Instance =
                        $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                    break;
            }

            var result = JsonSerializer.Serialize(problemDetails);
            context.Response.StatusCode = (int)problemDetails.Status;

            return context.Response.WriteAsync(result);
        }
    }
}
