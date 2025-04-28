var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // serve wwwroot (CSS)

app.MapGet("/", async context =>
{
    await context.Response.SendFileAsync("Pages/Index.cshtml");
});

app.Run();
