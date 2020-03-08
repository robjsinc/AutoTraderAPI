using AT.CustomExceptions;
using AT.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AT.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customException = exception as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "Unexpected error";
            var description = "Unexpected error";
            var path = context.Request.Path;

            if (null != customException)
            {
                message = customException.Message;
                description = customException.Description;
                statusCode = customException.Code;
                path = customException.Path;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                Message = message,
                Description = description,
                Path = path
            }));
        }
    }
}
