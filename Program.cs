using MicroML_AST_WebApp.AST;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // serve wwwroot

// GET request to show the form
app.MapGet("/", async context =>
{
    var html = await File.ReadAllTextAsync("Pages/Index.cshtml");
    await context.Response.WriteAsync(html);
});

// POST request to parse and show AST
app.MapPost("/", async context =>
{
    var form = await context.Request.ReadFormAsync();
    var code = form["code"];

    var parser = new Parser();
    AstNode ast;

    try
    {
        ast = parser.Parse(code!);
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"<h3>Error: {ex.Message}</h3>");
        return;
    }

    var html = $"""
    <html>
    <head><link rel="stylesheet" href="/site.css" /></head>
    <body>
    <h1>MicroML AST Viewer</h1>
    <form method="post">
        <textarea name="code
