﻿using Serilog;
using System.Text.Json;

namespace OrderManagementSystem.Api.Middleware;

public class HeartBeatMiddleware
{
    private readonly RequestDelegate next;

    public HeartBeatMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        Log.Information("HeaartBeat");
        if(context.Request.Path.StartsWithSegments("/hello"))
        {
            await context.Response.WriteAsync(JsonSerializer.Serialize("Hello from server"));
            context.Response.StatusCode = 202;
            return;

        }
        await next.Invoke(context);
    }
}
