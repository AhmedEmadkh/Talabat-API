﻿using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExcepetionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepetionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExcepetionMiddleware(RequestDelegate Next, ILogger<ExcepetionMiddleware> logger, IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var Response = _env.IsDevelopment() ? new APIExceptionResponse(500, ex.Message, ex.StackTrace) : new APIExceptionResponse(500);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var JsonResponse = JsonSerializer.Serialize(Response, Options);
                context.Response?.WriteAsync(JsonResponse);
            }
        }
    }
}
