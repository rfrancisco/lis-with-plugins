var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles(new StaticFileOptions { ServeUnknownFileTypes = true }); // Necessary to allow download of dll plugin files
app.UseRouting();
app.MapFallbackToFile("index.html");
app.MapGet("api/plugins", () => {
    var dir = Path.Combine(Environment.CurrentDirectory, "wwwroot/plugins/");
    var files = Directory.GetFiles(dir, "*.dll");
    return files.Select(f => Path.GetFileNameWithoutExtension(f)).OrderBy(f => f);
});
app.Run();
