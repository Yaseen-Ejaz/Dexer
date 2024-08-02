using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myprojajax.Data;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;
using Owin;
using Microsoft.Owin;
[assembly: OwinStartup(typeof(myprojajax.Startup))]



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddRazorPages();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddScoped<ApplicationDbContext>();

string connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseEndpoints(endpoints =>
//
    //endpoints.MapHub<ChatHub>("/notify");
//});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
