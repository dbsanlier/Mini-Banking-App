using System.Diagnostics;
using System.Text;
using bankingapp.API.Entities;
using bankingapp.API.Services;

namespace bankingapp.API.Middleware
{
    // Pipeline'in en disinda calisir: toplam sureyi ve son status code'u (hata dahil) gorur.
    // Her istek icin tek bir RequestLog satiri yazar; hata varsa (ExceptionHandlingMiddleware'in
    // context.Items["Exception"] alanina koydugu) ayni satira hata detaylari da eklenir.
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IRequestLogService requestLogService)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBodyAsync(context.Request);

            var originalResponseBody = context.Response.Body;
            using var responseBodyBuffer = new MemoryStream();
            context.Response.Body = responseBodyBuffer;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                var responseBody = await ReadResponseBodyAsync(responseBodyBuffer);
                responseBodyBuffer.Position = 0;
                await responseBodyBuffer.CopyToAsync(originalResponseBody);
                context.Response.Body = originalResponseBody;

                var exception = context.Items["Exception"] as Exception;

                var log = new RequestLog
                {
                    Method = context.Request.Method,
                    Path = context.Request.Path + context.Request.QueryString,
                    RequestBody = requestBody,
                    StatusCode = context.Response.StatusCode,
                    ResponseBody = responseBody,
                    SureMs = stopwatch.ElapsedMilliseconds,
                    ExceptionType = exception?.GetType().Name,
                    ExceptionMessage = exception?.Message,
                    StackTrace = exception?.StackTrace
                };

                await requestLogService.LogAsync(log);
            }
        }

        private static async Task<string?> ReadRequestBodyAsync(HttpRequest request)
        {
            if (request.ContentLength is null or 0)
            {
                return null;
            }

            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return string.IsNullOrEmpty(body) ? null : body;
        }

        private static async Task<string?> ReadResponseBodyAsync(MemoryStream responseBodyBuffer)
        {
            responseBodyBuffer.Position = 0;
            using var reader = new StreamReader(responseBodyBuffer, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            return string.IsNullOrEmpty(body) ? null : body;
        }
    }
}
