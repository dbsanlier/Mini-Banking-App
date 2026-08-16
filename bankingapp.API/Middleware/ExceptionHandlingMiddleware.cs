using System.Net;
using System.Text.Json;

namespace bankingapp.API.Middleware
{
    // Controller/Service katmanindan firlayan yakalanmamis hatalari yakalar,
    // istemciye tutarli bir { "message": "..." } govdesi doner.
    // Hatayi kendisi loglamaz; context.Items["Exception"] uzerinden
    // disaridaki RequestResponseLoggingMiddleware'in ayni istek satirina yazmasini saglar.
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Beklenmeyen bir hata olustu.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, string message)
        {
            context.Items["Exception"] = ex;

            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var body = JsonSerializer.Serialize(new { message });
            await context.Response.WriteAsync(body);
        }
    }
}
