using System.Configuration;
using GoogleAuthDemo.Areas.Identity.Pages.Account.Manage;
using GoogleAuthDemo.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddAuthentication().AddGoogle(options =>
{

    options.ClientId = "796963882724-sm8ak9lrj9fhvirotnp0tm9442h7gcer.apps.googleusercontent.com";
    options.ClientSecret = "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";

    options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
    options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
    options.Scope.Add("https://www.googleapis.com/auth/calendar");
    
    //options.Scope.Add("offline_access");
    options.AccessType = "offline";

    options.AuthorizationEndpoint += "?prompt=consent";

    options.SaveTokens = true;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


builder.Services.Configure<GoogleSettings>(configuration.GetSection(nameof(GoogleSettings)));
builder.Services.AddSingleton<IGoogleSettings>(s => s.GetRequiredService<IOptions<GoogleSettings>>().Value);
builder.Services.AddScoped<GoogleSettings>();


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
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
