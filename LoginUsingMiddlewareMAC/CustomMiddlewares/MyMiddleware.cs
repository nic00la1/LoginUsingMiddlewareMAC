using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace LoginUsingMiddlewareMAC.CustomMiddlewares;

public class MyMiddleware
{
    private readonly RequestDelegate _next;

    public MyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/" && context.Request.Method == "POST")
        {
            // Read response body as stream
            StreamReader reader = new StreamReader(context.Request.Body);
            string body = await reader.ReadToEndAsync();
            
            // Parse the request body from string into dictionary
            Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

            
            // Extract email and password from the dictionary
            queryDict.TryGetValue("email", out StringValues emailValues);
            queryDict.TryGetValue("password", out StringValues passwordValues);
            
            string? email = emailValues.FirstOrDefault();
            string? password = passwordValues.FirstOrDefault();
            
           // Check if email and password are submitted
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Email and password are required!");
                return;
            }
           else
            {
                // Check if email is valid 
                if (email != "admin@example.com")
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Email is invalid!");
                    return;
                }

                // Check if password is valid
                if (password != "admin1234")
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Password is invalid!");
                    return;
                }
            }

            // If email and password are valid, show success message
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Login successful!");
            return;
        }
        
        await _next(context);   
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyMiddleware>();
    }
}

