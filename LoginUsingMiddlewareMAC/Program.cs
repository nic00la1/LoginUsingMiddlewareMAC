using LoginUsingMiddlewareMAC.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Invoking my Custom Middleware
app.UseMyMiddleware();

app.Run(async context =>
{
    await context.Response.WriteAsync("No response :x");
});
app.Run();