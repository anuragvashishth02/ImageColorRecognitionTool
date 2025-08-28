using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Needed for Request.Scheme / Request.Host in controller
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseRouting();

// Serve static files from wwwroot
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "page1.html" }  // default landing page
});
app.UseStaticFiles();

// Serve Uploads folder as /Uploads
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});

// Map API Controllers
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    // ✅ Redirect root "/" always to page1.html
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("/page1.html");
    });
});

app.Run();
