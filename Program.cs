@*using Microsoft.EntityFrameworkCore;
using ToolCaptureAppClean.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ✅ SQLite file OUTSIDE OneDrive to avoid locks (adjust path if you like)
var dbPath = Path.Combine("C:\\Data", "ToolCaptureAppClean");
Directory.CreateDirectory(dbPath);
var conn = $"Data Source={Path.Combine(dbPath, "toolcapture.db")}";

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite(conn));

var app = builder.Build();

// ✅ Static files for /wwwroot/uploads
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
//optional to add back 
// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToolTests}/{action=Index}/{id?}");

app.Run();*@

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Important for Render: listen on port 5000 on all interfaces
app.Urls.Add("http://0.0.0.0:5000");

app.Run();


