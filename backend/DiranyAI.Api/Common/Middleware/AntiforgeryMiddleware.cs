using Microsoft.AspNetCore.Antiforgery;
using DiranyAI.Api.Authentication;

namespace DiranyAI.Api.Common.Middleware;

public class AntiforgeryMiddleware
{
    private readonly RequestDelegate _next;

    public AntiforgeryMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IAntiforgery antiforgery)
    {
        var endpoint = context.GetEndpoint();

        var skipAntiforgery =
            endpoint?.Metadata.GetMetadata<SkipAntiforgeryAttribute>() is not null;
        if (!skipAntiforgery &&
            (HttpMethods.IsPost(context.Request.Method) ||
            HttpMethods.IsPut(context.Request.Method) ||
            HttpMethods.IsPatch(context.Request.Method) ||
            HttpMethods.IsDelete(context.Request.Method)))
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
            }
            catch (AntiforgeryValidationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
        }

        await _next(context);
    }
}