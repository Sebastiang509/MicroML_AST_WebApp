using MicroML_AST_WebApp.AST;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles(); // Serve site.css from wwwroot/

// GET request: Show form with pre-filled MicroML code
app.MapGet("/", async context =>
{
    var defaultCode = "fun x -> x + 1";

    var html = "<html><head><link rel=\\\"stylesheet\\\" href=\\\"/site.css\\\" /></head><body>";
    html += "<h1>MicroML AST Viewer</h1>";
    html += "<form method='post'>";
    html += $"<textarea name='code' rows='10' cols='80'>{defaultCode}</textarea><br/>";
    html += "<button type='submit'>Parse</button></form>";
    html += "<div class='ast-tree'></div></body></html>";

    await context.Response.WriteAsync(html);
});

// POST request: Parse MicroML and display AST
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

    var html = "<html><head><link rel=\\\"stylesheet\\\" href=\\\"/site.css\\\" /></head><body>";
    html += "<h1>MicroML AST Viewer</h1>";
    html += "<form method='post'>";
    html += $"<textarea name='code' rows='10' cols='80'>{code}</textarea><br/>";
    html += "<button type='submit'>Parse</button></form>";
    html += "<div class='ast-tree'>";
    html += RenderAst(ast);
    html += "</div></body></html>";

    await context.Response.WriteAsync(html);
});

app.Run();

// Recursive helper to render AST nodes as nested <ul>/<li>
string RenderAst(AstNode node)
{
    var html = $"<ul><li><div>{node.Value}</div>";
    foreach (var child in node.Children)
    {
        html += RenderAst(child);
    }
    html += "</li></ul>";
    return html;
}

