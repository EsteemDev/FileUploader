using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

int LimitMB = 512;

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = LimitMB * 1024 * 1024;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = LimitMB * 1024 * 1024;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
