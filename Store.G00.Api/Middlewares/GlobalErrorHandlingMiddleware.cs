using Domain.Exceptions;
using Shared.ErrorModels;

namespace Store.G00.Api.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next,ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if(context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                // Log Exception
                _logger.LogError(ex, ex.Message);

                // 1.Set Status Code For response
                await HandlingErrorAsync(context, ex);

            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            // 2.Set Content Type Code For Response 
            // 3.Response Object (Body)
            // 4.return Response 

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => context.Response.StatusCode = StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End Point With {context.Request.Path} Not Found"
            };
            await context.Response.WriteAsJsonAsync(response);  /// Return Response
        }
    }
}
